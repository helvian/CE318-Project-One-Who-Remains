using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Drone stats and behaviours on death and on hit
 */

public class Drone : MonoBehaviour {

	public GameObject popupTextPrefab; //floating combat text canvas
	private GameObject player;
	public TextController tc;
	public GameController gc;
	public GameObject[] pickupObjects;

	private Vector3 offset = new Vector3 (0, -12, 0); //distance where powerups will spawn (on ground)

	public float moveSpeed = 4f;
	public float health = 20f;
	public int damage = 3;
	public int chanceForPickUp = 6; //chance is 1/chanceForPowerup
	public int chanceForWeapon = 3; //chance is 1/chanceForWeapon
	private int weaponRoll;
	private int moneyWorth = 60;
	public bool dead = false;

	void Start() {
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
		chanceForPickUp = (int)Mathf.Round(chanceForPickUp*GameController.difficultyModifier);
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
		}
	}

	//reduce health if hit by player
	public void TakeDamage (float amount) {
		if (!dead) {
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

	//roll for dropping a powerup or weapon on death, grant money, play death sound
	void Death(){
		if (Random.Range (0, chanceForPickUp) == 0) {
			MakePickUp (0);
		}
		if (Random.Range (0, chanceForWeapon) == 0) {
			weaponRoll = Random.Range (0, 100);
			if (weaponRoll >= 90) {
				MakePickUp (1);
			} else if (weaponRoll >= 80) {
				MakePickUp (2);
			} else if (weaponRoll >= 60) {
				MakePickUp (3);
			} else if (weaponRoll >= 40) {
				MakePickUp (4);
			} else if (weaponRoll >= 20) {
				MakePickUp (5);
			} else {
				MakePickUp (6);
			} 
		}
		player.GetComponent<Inventory> ().money += moneyWorth;
		tc.updateMoney (player.GetComponent<Inventory> ().money);
		GameObject.FindGameObjectWithTag ("Death Sound Player").GetComponent<PlayGlobalSound> ()
			.sources [1].Play ();
		dead = true;
	}

	//instantiate a powerup based on the rolls above 
	//make it a child of the clean-up object PickUp
	void MakePickUp(int powerUpIndex) {
		GameObject myPickup = Instantiate (pickupObjects[powerUpIndex], transform.position + offset, Quaternion.identity) as GameObject;
		GameObject pickupParent = GameObject.FindGameObjectWithTag ("Pickup Parent");
		myPickup.transform.parent = pickupParent.transform;
	}
}
