using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Script for fading in/out scenes
 */

public class Fader : MonoBehaviour {

	public Texture2D fadeTexture;
	public float fadeSpeed = 0.5f; //how fast to fade

	private float alpha = 1.0f; //alpha of the texture
	private int fadeDirection = -1; //-1 = fade in, 1 = fade out

	void Awake() {
		SceneManager.sceneLoaded += StartFade; //automatically fade in on every loaded scene
	}

	void StartFade (Scene scene, LoadSceneMode mode) {
		Fade (-1);
	}

	void OnGUI() {
		//adjust the alpha of the GUI and fadeTexture according to the direction
		alpha += fadeDirection * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp (alpha, 0, 1);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}

	//public function used to change the fade direction
	public float Fade(int direction) {
		fadeDirection = direction;
		return fadeSpeed;
	}


}
