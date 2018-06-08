using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Missile launching behaviours done by the boss
 */

public class BossBehaviour : MonoBehaviour {

	public GameObject missile; //missile object to spawn
	public Transform[] missileSpawns; //where they spawn from 
	public GameObject missileParent; //cleanup object 
	public float missileWait; //time between missiles
	public AudioSource missileLaunch; //missile launch noise

	void Start () {
		missileLaunch = gameObject.AddComponent<AudioSource> ();
		missileLaunch.clip = Resources.Load ("Sounds/missilelaunch") as AudioClip;
		missileParent = GameObject.FindGameObjectWithTag ("Missile Parent");
		StartCoroutine (LaunchMissiles ());
	}

	//launch a missile, wait N seconds, launch another, wait again
	//attach missiles to cleanup object
	IEnumerator LaunchMissiles () {
		while (true) {
			missileLaunch.Play ();
			GameObject missile1 = Instantiate (missile, missileSpawns [0].position, Quaternion.Euler(Random.insideUnitSphere)) 
				as GameObject;
			missile1.transform.parent = missileParent.transform;
			yield return new WaitForSeconds (missileWait);
			missileLaunch.Play ();
			GameObject missile2 = Instantiate (missile, missileSpawns [1].position, Quaternion.Euler(Random.insideUnitSphere))
				as GameObject;
			missile2.transform.parent = missileParent.transform;
			yield return new WaitForSeconds (missileWait);
		}
	}

}
