using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/*
 * Object tracking collisions for the player's helicopter
 */

public class Helicopter : MonoBehaviour {

	public int health = 5;
	public TextController tc;
	public GameObject particleParent; //cleanup object
	public ParticleSystem explosion; 

	void Start() {
		particleParent = GameObject.FindGameObjectWithTag ("Particle Parent");
	}

	void Update() {
		if (health <= 0) {
			StartCoroutine (GameOver ());
		}
	}

	//end game if helicopter dies
	IEnumerator GameOver() {
		float loadDelay = GameObject.Find ("Fader").GetComponent<Fader> ().Fade (1);
		yield return new WaitForSeconds (loadDelay);
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene (0);	
	}

	//if a missile hits the helicopter, lower the health
	private void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Missile") {
			health--;
			tc.updateHelicopterHealth (health);
			other.gameObject.GetComponent<MissileSeek> ().Death ();
		}
	}
}
