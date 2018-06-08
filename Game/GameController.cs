using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/* 
 * Main brain for the game controlling enemy wave spawns and many other input related things
 */

public class GameController : MonoBehaviour {

	public GameObject[] enemy;

	public static float difficultyModifier = 1.0f;
	public int wave;
	public float enemiesThisWave;
	public int enemiesSpawnedThisWave;
	public int droneRoll;

	public float spawnWait; //time between enemy spawns
	public float waveWait; //time between waves
	public float startWait; //time between start of the game and first enemy

	public Transform[] spawnLocs;
	private GameObject myEnemy;

	public GunController gunC;
	public TextController tc;
	public Image pausePanel;
	public Text pauseText;

	void Start () {
		enemiesThisWave = 4;
		tc = GetComponent<TextController> ();
		pausePanel = GameObject.FindGameObjectWithTag ("Pause Panel").GetComponent<Image>();
		pauseText = GameObject.FindGameObjectWithTag ("Pause Panel").transform.FindChild("Pause Text").GetComponent<Text>();
		tc.updateWave (wave);
		//spawn waves if it is not the tutorial
		if (wave != -1) {
			StartCoroutine (SpawnWaves ());
		}
	}

	void Update() {
		//pause input, fade in/out the pause screen on the press of P
		if (Input.GetKeyDown (KeyCode.P) && Time.timeScale == 1) {
			Time.timeScale = 0;
			pausePanel.color = new Color (0f, 0f, 0f, 1f);
			pauseText.color = new Color (0f, 0f, 0f, 1f);
		} else if (Input.GetKeyDown (KeyCode.P) && Time.timeScale == 0) {
			Time.timeScale = 1;
			pausePanel.color = new Color (0f, 0f, 0f, 0f);
			pauseText.color = new Color (0f, 0f, 0f, 0f);
		}
	}

	//wave spawning
	IEnumerator SpawnWaves() {
		yield return new WaitForSeconds (startWait);
		while (true) {
			//if all enemies this wave have been spawned advance wave and play new wave sound
			if (enemiesSpawnedThisWave >= enemiesThisWave) {
				nextWave ();
				GameObject.FindGameObjectWithTag ("Death Sound Player").GetComponent<PlayGlobalSound> ()
					.sources [3].Play ();
				enemiesSpawnedThisWave = 0;
				yield return new WaitForSeconds (waveWait);
			} else {
				//see if we spawn a mutant or a drone, then spawn it and parent it to the clean-up object
				droneRoll = Random.Range (0, 100);
				if (droneRoll > 80) {
					myEnemy = Instantiate (enemy [1], spawnLocs [Random.Range (0, spawnLocs.Length)].position, Quaternion.identity) 
						as GameObject;
				} else {
					myEnemy = Instantiate (enemy [0], spawnLocs [Random.Range (0, spawnLocs.Length)].position, Quaternion.identity) 
					as GameObject;
				}
				GameObject enemyParent = GameObject.FindGameObjectWithTag ("Enemy Parent");
				myEnemy.transform.parent = enemyParent.transform;
				enemiesSpawnedThisWave++;
				yield return new WaitForSeconds (spawnWait);
			}
		}
	}

	//if player dies, go back to menu
	public void GameOver() {
		StartCoroutine (BackToMenu ());
	}

	//invoke fader script
	IEnumerator BackToMenu() {
		float waitTime = GameObject.Find ("Fader").GetComponent<Fader> ().Fade (1);
		yield return new WaitForSeconds (waitTime);
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene(0);
	}

	//proceed to next wave
	void nextWave()	 {
		wave++;
		waveWait = Mathf.Clamp ((waveWait - 0.1f), 1f, 5f);
		enemiesThisWave += 4;
		tc.updateWave (wave);
	}
}
