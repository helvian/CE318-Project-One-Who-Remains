using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*
 * Script responsible for switching weapons
 */

public class WeaponSwitcher : MonoBehaviour {

	public int chosenWeapon; //the index of the held gun in the inventory
	public Inventory inv;
	public GameObject gunParent; //the game object holding the gun models
	public Transform gunModel; //the models themselves
	public Gun statsOfNewGun; //stats to be written by TextController
	public TextController tc;
	Transform currentGun;
	public int sceneIndex;

	void Start() {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.sceneLoaded += OnLevelLoad;
		inv = GetComponent<Inventory> ();
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
		gunParent = GameObject.FindGameObjectWithTag ("Gun Parent");
		ChooseWeapon ();
	}

	//re-read the inventory on scene change from Main Scene
	void OnLevelLoad(Scene scene, LoadSceneMode mode) {
		if (sceneIndex == 2) {
			inv = GetComponent<Inventory> ();
			tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
			gunParent = GameObject.FindGameObjectWithTag ("Gun Parent");
			ChooseWeapon ();
		}
	}

	//watch for weapon change inputs, change the index of the held gun accordingly
	void Update () {
		int lastWeapon = chosenWeapon;
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) {
			if (chosenWeapon == Inventory.slots - 1) {
				chosenWeapon = -1;
			}
			chosenWeapon++;
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) {
			if (chosenWeapon == 0) {
				chosenWeapon = Inventory.slots;
			}
			chosenWeapon--;
		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			chosenWeapon = 0;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			chosenWeapon = 1;
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			chosenWeapon = 2;
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			chosenWeapon = 3;
		}
		if (lastWeapon != chosenWeapon) {
			ChooseWeapon ();
		}
	}

	public void ChooseWeapon() {
		//find the currently equipped weapon and hide it
		for (int i = 0; i < 6; i++) { 
			if (gunParent.transform.GetChild (i).gameObject.activeSelf == true) {
				currentGun = gunParent.transform.GetChild (i);
				break;
			}
		}
		currentGun.gameObject.GetComponent<GunController> ().reloading = false;
		currentGun.gameObject.SetActive (false);

		//find the desired weapon and show it
	 	string type = inv.guns [chosenWeapon].gunClass.ToString ();
		gunModel = gunParent.transform.FindChild(type);
		gunModel.gameObject.SetActive (true);

		//print out the new stats
		statsOfNewGun = inv.guns [chosenWeapon];
		tc.updateWeaponStats (statsOfNewGun);
	}
}
