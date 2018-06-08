using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Similar shop function to Shop, but for ending the level
 */

public class StageEndShop : MonoBehaviour {

	public Inventory inv;
	public TextController tc;
	public GameObject player;
	private int cost;
	private bool canBuy;

	void Start() {
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		cost = 10000;
	}

	void Update() {
		if (canBuy) {
			if (Input.GetKeyDown (KeyCode.F)) {
				StartCoroutine(EndStage ());
			}
		}
	}

	//invoke fade script and transfer the player to the next level
	IEnumerator EndStage() {
		if (inv.money < cost) {
			tc.InsufficientFundsText ();
		} else {
			inv.money -= cost;
			float loadDelay = GameObject.Find ("Fader").GetComponent<Fader> ().Fade (1);
			yield return new WaitForSeconds (loadDelay);
			player.transform.position = new Vector3 (0, 2, -2);
			SceneManager.LoadScene (3);	
		}
	}

	private void OnTriggerEnter() {
		tc.showEndGameText (cost);
		canBuy = true;
	}

	private void OnTriggerExit() {
		tc.hideContextText ();
		canBuy = false;
	}
}

