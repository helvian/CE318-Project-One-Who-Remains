using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

/*
 * Changes certain values in the option menu
 */

public class OptionsMenu : MonoBehaviour {

	public AudioMixer au;

	//change the music volume based on the music slider's position
	public void setMusicVol (float vol) {
		au.SetFloat ("MusicVol", vol);
	}

	//change the music volume based on the sound slider's position
	public void setSoundVol (float vol) {
		au.SetFloat ("SoundVol", vol);
	}

	//change the quality based on the quality dropdown
	public void setQuality (int quality) {
		QualitySettings.SetQualityLevel (quality);
	}

	//change the game difficulty based on the difficulty dropdown
	public void setDifficulty (int difficulty) {
		//easy
		if (difficulty == 0) {
			GameController.difficultyModifier = 1.0f;
		} 
		//hard
		else if (difficulty == 1) {
			GameController.difficultyModifier = 1.5f;
		}
	}
}