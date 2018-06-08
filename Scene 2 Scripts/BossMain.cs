using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Boss health and death monitor
 */

public class BossMain : MonoBehaviour {

	public float totalHealth = 3000f; //how much health the boss has overall

	public ParticleSystem explosion; //explosion on death
	public TextController tc;
	public GameObject particleParent; //cleanup object

	void Start() {
		particleParent = GameObject.FindGameObjectWithTag ("Particle Parent");
		tc.updateBossHealth (totalHealth);
	}

	//when hit, reduce total health, check for death
	public void UpdateTotalHealth (float amount) {
		totalHealth -= amount;
		tc.updateBossHealth (totalHealth);
		if (totalHealth <= 0) {
			StartCoroutine(Death ());
		}
	}

	//explode, invoke Fader, go back to main menu
	IEnumerator Death() {
		ParticleSystem spawnedExplosion = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
		spawnedExplosion.transform.parent = particleParent.transform;
		spawnedExplosion.Play ();
		float loadDelay = GameObject.Find ("Fader").GetComponent<Fader> ().Fade (1);
		yield return new WaitForSeconds (loadDelay);
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene (0);	
		Destroy (gameObject);
	}

}
