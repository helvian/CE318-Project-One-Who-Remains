using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Melee hit detection used by mutants
 */

public class MeleeDetection : MonoBehaviour {

	Mutant m; //the mutant that is attacking

	void Start() {
		m = GetComponentInParent<Mutant> ();
	}

	//if the player is inside the collision box, damage them
	private void OnTriggerEnter(Collider other) {
		PlayerController pc = other.GetComponent<PlayerController> ();
		pc.TakeDamage (m.damage, "Mutant");
	}
}
