using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Gives the player a gun if they have enough funds for a purchase at a shop 
 */

public class Shop : MonoBehaviour {

	public Inventory inv; //reference to the player's held guns
	public Gun itemForSale; //reference to the gun sold by this shop currently
	public GameController gc;
	public TextController tc;
	public WeaponSwitcher ws;
	public GameObject statObjectParent; //reference to the gun stats held in secondary location

	private int heldGun; //what gun the player is holding
	private int cost;
	public string gunClass;
	private bool canBuy; 

	// Use this for initialization
	void Start () {
		gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
		ws = GameObject.FindGameObjectWithTag ("Player").GetComponent<WeaponSwitcher> ();
		statObjectParent = GameObject.FindGameObjectWithTag ("Stat Object");
		itemForSale = GetComponent<Gun> ();
		itemForSale.CreateGun (gunClass, gc.wave);
	}

	//if player is near enough to the shop, let them buy
	void Update() {
		if (canBuy) {
			if (Input.GetKeyDown (KeyCode.F)) {
				itemForSale.CreateGun (gunClass, gc.wave);
				PurchaseGun ();
			}
		}
	}
		
	void PurchaseGun() {
		//validate the purchase
		if (inv.money < cost) {
			tc.InsufficientFundsText ();
		} else {
			//trade the player's held gun for the one in the shop
			heldGun = ws.chosenWeapon;
			inv.RemoveGun (heldGun);
			inv.AddGun (itemForSale, heldGun);
			inv.money -= cost;
			ws.ChooseWeapon ();
			WriteToStatObject ();
		}
	}

	//copy over all stats of sold gun to the stat object
	void WriteToStatObject() {
		GameObject statObject = statObjectParent.transform.FindChild ("Slot " + heldGun).gameObject;
		Gun statObjectStats = statObject.GetComponent<Gun> ();
		statObjectStats.damage = itemForSale.damage;
		statObjectStats.accuracy = itemForSale.accuracy;
		statObjectStats.pelletCount = itemForSale.pelletCount;
		statObjectStats.fireRate = itemForSale.fireRate;
		statObjectStats.ammoCount = itemForSale.ammoCount;
		statObjectStats.ammoSize = itemForSale.ammoSize;
		statObjectStats.reserveAmmo = itemForSale.reserveAmmo;
		statObjectStats.gunClass = itemForSale.gunClass;
		statObjectStats.firstMod = itemForSale.firstMod;
		statObjectStats.secondMod = itemForSale.secondMod;
		statObjectStats.rarity = itemForSale.rarity;
		statObjectStats.level = itemForSale.level;
		statObjectStats.reloadTime = itemForSale.reloadTime;
	}

	//show text telling player that they can buy
	private void OnTriggerEnter() {
		cost = gc.wave * 300;
		tc.showContextText (itemForSale, cost);
		canBuy = true;
	}

	//hide text
	private void OnTriggerExit() {
		tc.hideContextText ();
		canBuy = false;
	}


}
