using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Helper script that removes objects marked as DontDestroyOnLoad that must
 * be destroyed (when going back to menu)
 */

public class DestroyPlayer : MonoBehaviour {

	void Start () {
		Destroy (GameObject.Find ("Player"));
		Destroy (GameObject.Find ("Gun Stats"));
	}

}
