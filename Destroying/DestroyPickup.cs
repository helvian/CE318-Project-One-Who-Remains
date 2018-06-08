using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script used to destroy pickups after they spawn so they do not clutter the field
 */

public class DestroyPickup : MonoBehaviour {
	public float duration;

	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyTimer ());
	}
	
	IEnumerator DestroyTimer() {
		yield return new WaitForSeconds (duration);
		Destroy (gameObject);
	}
}
