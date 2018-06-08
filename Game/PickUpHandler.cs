using UnityEngine;
using System.Collections;

/*
 * Handles regular pickups (not weapons) 
 */

public class PickUpHandler : MonoBehaviour {

	public enum PickUp
	{Ammo, Health, Armour, Money}; //the kinds of bonuses a pickup can give
	public PickUp pu;

	public int healthRestore = 50;
	public int ammoRestore;
	public int armourRestore = 50;
	public int moneyWorth = 100;

	private AudioSource au;

	public TextController tc;

	void Start () {
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
		pu = (PickUp)Random.Range (0, 4);
		au = GetComponent<AudioSource> ();
	}

	//only the player can collide with powerups, give them a bonus based on the enum above
	private void OnTriggerEnter (Collider other) {
		au.Play ();
		if (pu == PickUp.Ammo) {
			GunController gun = other.GetComponentInChildren<GunController> ();
			AmmoPickUp (gun);
		} else if (pu == PickUp.Health) {
			PlayerStats stats = other.GetComponent<PlayerStats> ();
			HealthPickUp (stats);
		} else if (pu == PickUp.Armour) {
			PlayerStats stats = other.GetComponent<PlayerStats> ();
			ArmourPickUp (stats);
		} else if (pu == PickUp.Money) {
			Inventory inv = other.GetComponent<Inventory> ();
			MoneyPickUp (inv);
		} 
		//play pickup sound
		GameObject.FindGameObjectWithTag ("Death Sound Player").GetComponent<PlayGlobalSound> ()
			.sources [2].Play ();
		Destroy (gameObject);
	}

	//give player 3 magazines worth or ammo or 6 bullets, whichever is bigger
	void AmmoPickUp(GunController g){
		Gun gun = g.gun;
		ammoRestore = Mathf.Max(6, gun.ammoSize * 3);
		gun.reserveAmmo += ammoRestore;
		tc.updateReserveAmmo (gun.reserveAmmo);
	}

	//give player 50 health
	void HealthPickUp (PlayerStats stats) {
		stats.health += healthRestore;
		if (stats.health > stats.maxHealth) {
			stats.health = stats.maxHealth;
		}
		tc.updateHealth (stats.health);
	}

	//give player 50 armour
	void ArmourPickUp (PlayerStats stats) {
		stats.armour += armourRestore;
		if (stats.armour > stats.maxArmour) {
			stats.armour = stats.maxArmour;
		}
		tc.updateArmour (stats.armour);
	}

	//give player 100 money
	void MoneyPickUp (Inventory inv) {
		inv.money += moneyWorth;
		tc.updateMoney (inv.money);
	}
}
