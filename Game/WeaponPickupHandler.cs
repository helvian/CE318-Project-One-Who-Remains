﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles weapon pickups, spawning them in and changing their look based on the gun created
 */

public class WeaponPickupHandler : MonoBehaviour {

	public Gun gun; //the gun that represents this pickup
	private AudioSource au;
	private ParticleSystem.MainModule beam; //the beam denoting rarity coming out of the gun
	public string gunClass;
	public Inventory inv; //the player's inventory that the pickup will go into
	public int heldGun; //what gun the player is holding (index of inventory)
	public WeaponSwitcher ws;
	public TextController tc;
	private int currentWave;
	public GameObject statObjectParent;


	void Start () {
		inv = GameObject.FindGameObjectWithTag ("Player").GetComponent<Inventory> ();
		heldGun = GameObject.FindGameObjectWithTag ("Player").GetComponent<WeaponSwitcher> ().chosenWeapon;
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
		ws = GameObject.FindGameObjectWithTag ("Player").GetComponent<WeaponSwitcher> ();
		currentWave = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().wave;
		gun = GetComponent<Gun> ();	
		au = GetComponent<AudioSource> ();
		beam = GetComponentInChildren<ParticleSystem> ().main;
		statObjectParent = GameObject.FindGameObjectWithTag ("Stat Object");
		gun.CreateGun (gunClass, currentWave);
		//change the beam colour based on the rarity made above
		string beamColour = gun.GetRarity ();
		switch (beamColour) {
		case ("Common"):
			break;
		case ("Uncommon"):
			beam.startColor = new Color (0f, 255f, 0f);
			break;
		case ("Rare"):
			beam.startColor = new Color (0f, 0f, 255f);
			break;
		case ("Epic"):
			beam.startColor = new Color (200f, 0f, 255f);
			break;
		case ("Legendary"):
			beam.startColor = new Color (255f, 65f, 0f);
			break;
		}
	}

	//monitor which gun the player is holding
	void Update() {
		heldGun = GameObject.FindGameObjectWithTag ("Player").GetComponent<WeaponSwitcher> ().chosenWeapon;
	}

	//trade player's held gun with the gun generated by this pickup
	private void OnTriggerEnter (Collider other) {
		au.Play ();
		WriteToStatObject ();
		Debug.Log("Deleting slot " + heldGun);
		inv.RemoveGun (heldGun);
		Debug.Log ("Adding " + gun + " to slot " + heldGun);
		inv.AddGun (gun, heldGun);
		ws.ChooseWeapon ();
		GameObject.FindGameObjectWithTag ("Death Sound Player").GetComponent<PlayGlobalSound> ()
			.sources [2].Play ();
		Destroy (gameObject);
	}

	//copy all stats of generated gun into secondary location
	void WriteToStatObject() {
		GameObject statObject = statObjectParent.transform.FindChild ("Slot " + heldGun).gameObject;
		Gun statObjectStats = statObject.GetComponent<Gun> ();
		statObjectStats.damage = gun.damage;
		statObjectStats.accuracy = gun.accuracy;
		statObjectStats.pelletCount = gun.pelletCount;
		statObjectStats.fireRate = gun.fireRate;
		statObjectStats.ammoCount = gun.ammoCount;
		statObjectStats.ammoSize = gun.ammoSize;
		statObjectStats.reserveAmmo = gun.reserveAmmo;
		statObjectStats.gunClass = gun.gunClass;
		statObjectStats.firstMod = gun.firstMod;
		statObjectStats.secondMod = gun.secondMod;
		statObjectStats.rarity = gun.rarity;
		statObjectStats.level = gun.level;
		statObjectStats.reloadTime = gun.reloadTime;
	}
}
