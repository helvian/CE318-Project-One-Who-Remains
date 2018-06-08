using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Script that ends the tutorial
 */

public class TutorialEnd : MonoBehaviour {

	//if the player presses T, end the tutorial
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			StartCoroutine(EndTutorial());
		}
	}

	//invoke Fader then switch scene to the main menu
	IEnumerator EndTutorial() {
		float waitTime = GameObject.Find ("Fader").GetComponent<Fader> ().Fade (1);
		yield return new WaitForSeconds (waitTime);
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene(0);
	}

}
