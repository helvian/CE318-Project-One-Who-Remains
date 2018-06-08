using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Helper script to signal that this object should not be destroyed on load 
 * (objects that do not already have a script)
 */

public class StatCarryover : MonoBehaviour {

	private int sceneIndex;

	void Start () {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		if (sceneIndex == 2) {
			DontDestroyOnLoad (transform.gameObject);
		}
	}

}
