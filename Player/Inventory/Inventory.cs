using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Object that stores the guns that the player is holding.
 * Stats are held in a separate game object to support holding multiples of the same weapon.
 */

public class Inventory : MonoBehaviour {

	public Gun[] guns = new Gun[slots]; //the guns in the inventory
	public const int slots = 4; //how many guns can be held
	public int money; //money held by player
	public int sceneIndex; //what scene is loaded

	//persistence between levels
	void Start () {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.sceneLoaded += ReReadStats;
	}

	//only re-read stats if this is the survival scene (Main Scene)
	//re-read stats so the next level knows the stats of each held gun
	void ReReadStats(Scene scene, LoadSceneMode mode) {
		if (sceneIndex == 2) {
			for (int i = 0; i < slots; i++) {
				guns [i] = GameObject.FindGameObjectWithTag ("Stat Object").transform.FindChild ("Slot " + i).GetComponent<Gun> ();
			}
		}
	}

	//add a gun to the inventory at the given index in guns
	public void AddGun(Gun newGun, int atIndex) {
		guns [atIndex] = newGun;
	}
		
	//remove a gun from the inventory at the given index in guns
	public void RemoveGun(int atIndex) {
		guns [atIndex] = null;
	}




}
