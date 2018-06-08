using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Handles player movement via Character Controller 
 */

public class PlayerController : MonoBehaviour {


	private Vector3 moveDirection = Vector3.zero; //initialise movement to zero

	public PlayerStats ps;
	public TextController tc;
	public GameController gc;
	public PlayGlobalSound pgs;

	public int sceneIndex;

	void Start () {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		if (sceneIndex == 2) {			
			DontDestroyOnLoad (transform.gameObject);
		}
		ps = GetComponent<PlayerStats> ();
		pgs = GetComponentInChildren<PlayGlobalSound> ();
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		tc.updateArmour (ps.armour);
		tc.updateHealth (ps.health);
	}

	//get movement inputs and check for a game over
	void Update () {
		if (ps.health <= 0) {
			gc.GameOver ();
		}
		tc.updateArmour (ps.armour);
		tc.updateHealth (ps.health);
		CharacterController cc = GetComponent<CharacterController> ();
		float x = Input.GetAxis("Horizontal");
		float z  = Input.GetAxis ("Vertical");

		moveDirection = new Vector3 (x, 0, z);
		moveDirection = transform.TransformDirection (moveDirection);
		moveDirection *= ps.speed;

		moveDirection.y -= ps.gravity * Time.deltaTime;
		cc.Move (moveDirection * Time.deltaTime);
	}

	//if player is hit, halve the damage and take out of armour
	//if not enough armour, bleed over into health
	//otherwise take straight out of health
	public void TakeDamage (int damage, string hitBy) {
		switch (hitBy) {
		case ("Drone"):
			pgs.sources [Random.Range (4, 6)].Play ();
			break;
		case ("Mutant"):
			pgs.sources [7].Play ();
			break;
		default:
			break;
		}
		if (ps.armour > 0 && ps.armour > damage) { 
			ps.armour = ps.armour - Mathf.RoundToInt(0.5f * damage);
		} 
		//if the player has armour but less than the incoming damage
		else if (ps.armour > 0) {
			int bleedoverDamage = damage - ps.armour;
			ps.armour = 0;
			ps.health -= bleedoverDamage;
		} 
		//if the player has no armour
		else {
			ps.health -= damage;
		}
	}
}
