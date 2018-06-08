using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Events to play on the press of different buttons on the start menu
 */

public class StartMenuScript : MonoBehaviour {

	int sceneIndex; 

	//begin the game
    public void StartGame(){
		sceneIndex = 2;
		StartCoroutine (ChangeLevel ());
    }

	//begin the tutorial
	public void StartTutorial() {
		sceneIndex = 1;
		StartCoroutine (ChangeLevel ());
	}

	//quit the game
    public void ExitGame(){
        Application.Quit();
    }

	//invoke Fader
	IEnumerator ChangeLevel() { 
		float waitTime = GameObject.Find ("Fader").GetComponent<Fader> ().Fade (1);
		yield return new WaitForSeconds (waitTime);
		SceneManager.LoadScene(sceneIndex);
	}
}
