using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script used for the hittable parts of the boss
 */

public class BossParts : MonoBehaviour {

	public float health; //how much health the part has

	public TextController tc;
	public BossMain main; //the boss parent object
	public Collider thisCollider;
	public ParticleSystem smoke; //particle fired when dead

	void Start() {
		health = main.totalHealth / 4;
		thisCollider = GetComponent<Collider> ();
	}

	//take damage when hit by player
	public void TakeDamage (float amount) {
		health -= amount;
		main.UpdateTotalHealth (amount);
		if (health <= 0) {
			Death ();
		}
	}

	//destroy the collider on this object and play the particle system
	void Death() {
		Destroy (thisCollider);
		smoke.Play ();
	}
}
