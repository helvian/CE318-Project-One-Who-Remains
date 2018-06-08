using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Mutant stats and behaviours on death and on hit
 */

public class Mutant : MonoBehaviour {

	public GameObject popupTextPrefab; //floating combat text canvas
	private GameObject player;
	public TextController tc;
	public GameController gc;
	public GameObject[] pickupObjects;

	private Vector3 offset = new Vector3 (0, 1, 0); //distance where powerups will spawn (not inside the ground)

	public float moveSpeed = 4f;
	public float health = 50f;
	public int damage = 20;
	public int chanceForPowerup = 6; //chance is 1/chanceForPowerup
	public int chanceForWeapon = 4; //chance is 1/chanceForWeapon
	private int weaponRoll;
	private int moneyWorth = 60;

	private Animator an;

	void Start() {
		an = GetComponent<Animator> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		tc = gc.GetComponent<TextController> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		ApplyDifficultyMod ();
		ApplyWaveMod ();
	}

	//adjust stats based on difficulty chosen on menu
	void ApplyDifficultyMod () {
		health *= GameController.difficultyModifier;
		moveSpeed *= GameController.difficultyModifier;
		damage = Mathf.RoundToInt(damage * GameController.difficultyModifier);
		chanceForPowerup = (int)Mathf.Round(chanceForPowerup*GameController.difficultyModifier);
		chanceForWeapon = (int)Mathf.Round(chanceForWeapon*GameController.difficultyModifier);
	}

	//adjust stats based on how far in the game it is
	void ApplyWaveMod() {
		//if it is the tutorial, give it a lot of health
		if (gc.wave == -1) {
			health = 999999;
		} else {
			health = (health * (gc.wave * 0.2f)) + 40;
			moveSpeed = Mathf.Clamp (moveSpeed + (gc.wave * 0.2f), 4f, 10f);
			damage = Mathf.RoundToInt (Mathf.Clamp (gc.wave * 1.1f, 20f, 67f));
		}
	}

	//take damage if hit by player
	public void TakeDamage (float amount) {
		if (an.GetBool ("Dead") == false) {
			initPUT (amount.ToString ());
			health -= amount;
			if (health <= 0) {
				Death ();
			}
		}
	}

	//floating combat text generator
	//Code based off of Lena Florian's tutorial:
	//URL: https://www.youtube.com/watch?v=bFLtH05SO4M&t=321s
	void initPUT(string text) {
		//create a popup text object holding the damage and adjust the RectTransform of the enemy canvas
		//so that it faces the same way the enemy is facing
		GameObject temp = Instantiate (popupTextPrefab) as GameObject;
		RectTransform tempRect = temp.GetComponent<RectTransform> ();
		temp.transform.SetParent (transform.FindChild ("Enemy Canvas"));
		tempRect.transform.localPosition = popupTextPrefab.transform.localPosition;
		tempRect.transform.localScale = popupTextPrefab.transform.localScale;
		tempRect.transform.localRotation = popupTextPrefab.transform.localRotation;
		tempRect.transform.localRotation *= Quaternion.Euler (0, 180, 0);

		temp.GetComponent<Text> ().text = text;
		Destroy (temp, 1.1f);
	}

	//roll for dropping a powerup or weapon on death, grant money, play death sound and animation
	void Death(){
		if (Random.Range (0, chanceForPowerup) == 0) {
			MakePowerup (0);
		}
		if (Random.Range (0, chanceForWeapon) == 0) {
			weaponRoll = Random.Range (0, 100);
			if (weaponRoll >= 90) {
				MakePowerup (1);
			} else if (weaponRoll >= 80) {
				MakePowerup (2);
			} else if (weaponRoll >= 60) {
				MakePowerup (3);
			} else if (weaponRoll >= 40) {
				MakePowerup (4);
			} else if (weaponRoll >= 20) {
				MakePowerup (5);
			} else {
				MakePowerup (6);
			} 
		}
		player.GetComponent<Inventory> ().money += moneyWorth;
		tc.updateMoney (player.GetComponent<Inventory> ().money);
		GameObject.FindGameObjectWithTag ("Death Sound Player").GetComponent<PlayGlobalSound> ()
			.sources [0].Play ();
		an.SetBool ("Dead", true);
		}

	//instantiate a powerup based on the rolls above 
	//make it a child of the clean-up object PickUp
	void MakePowerup(int powerUpIndex) {
		GameObject myPickup = Instantiate (pickupObjects[powerUpIndex], transform.position + offset, Quaternion.identity) as GameObject;
		GameObject pickupParent = GameObject.FindGameObjectWithTag ("Pickup Parent");
		myPickup.transform.parent = pickupParent.transform;
	}
}
