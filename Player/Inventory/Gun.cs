using UnityEngine;
using System.Collections;

/*
 * Object holding stats for guns
 * Randomly generated stats 
 */

public class Gun : MonoBehaviour {

	//what kind of gun it is = what baseline stats it has
	public enum Class 
	{
		Pistol, SMG, AssaultRifle, LMG, Shotgun, Sniper
	};

	//stat bonuses
	public enum Modifier
	{
		None, DamagePlus, FireRatePlus, MagPlus, ReloadPlus, PelletPlus
	};

	//overall stat amplifiers
	public enum Rarity 
	{
		Common, Uncommon, Rare, Epic, Legendary
	};

	//These stats are base stats, each class will modify each stat according to the archetype
	public float damage; //damage done on hit
	public float fireRate; //a fireRate of 5 means 5 shots per second
	public int pelletCount; //how many raycasts made per click
	public float accuracy; //how large the bullet spread is, an accuracy of 0 is perfect accuracy

	public int ammoCount; //ammo in the current magazine
	public int ammoSize; //maximum ammo in a magazine

	public int reserveAmmo; //ammo held in reserve

	public float reloadTime; //seconds needed to reload

	public float range = 100f; //raycast limiter 

	//Variables part of the borderlands-esque gun generation

	public Class gunClass;
	public Modifier firstMod;
	public Modifier secondMod;
	public Rarity rarity;

	//What level it is, amplifying its stats the higher it is
	public int level;

	public float firstModChance;
	public float secondModChance;

	//persistence between levels
	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
	}

	//generate a gun based on the chosen class and the current wave
	public void CreateGun(string gunClass, int wave) {
		Class gc = (Class) System.Enum.Parse( typeof (Class), gunClass);
		level = wave;
		DetermineGunClass (gc);
		DetermineStats ();
		DetermineModifiers ();
		DetermineRarity ();
		Clamping ();
	}

	//Based on the class, assign baseline stats to be later modified
	void DetermineGunClass(Class gc) {
		gunClass = gc;
		switch (gunClass) {
		case (Class.Pistol):
			damage = 10f;
			fireRate = 5f;
			accuracy = 0.05f;
			pelletCount = 1;
			ammoCount = 12;
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime = 1.5f;
			break;
		case (Class.SMG):
			damage = 5f;
			fireRate = 12f;
			accuracy = 0.1f;
			pelletCount = 1;
			ammoCount = 30;
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime = 2.2f;
			break;
		case (Class.AssaultRifle):
			damage = 12f;
			fireRate = 7f;
			accuracy = 0.05f;
			pelletCount = 1;
			ammoCount = 30;
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime = 2.7f;
			break;
		case (Class.LMG):
			damage = 16f;
			fireRate = 6f;
			accuracy = 0.15f;
			pelletCount = 1;
			ammoCount = 60;
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime = 4f;
			break;
		case (Class.Shotgun):
			damage = 6f;
			fireRate = 2f;
			accuracy = 0.2f;
			pelletCount = 8;
			ammoCount = 6;
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime = 2f;
			break;
		case (Class.Sniper):
			damage = 150f;
			fireRate = 1f;
			accuracy = 0f;
			pelletCount = 1;
			ammoCount = 1;
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime = 3f;
			break;
		}
	}

	void DetermineStats() {
		damage += ((level * 1.4f) * 1.1f);
		fireRate += (level * 0.2f);
		ammoCount += Mathf.RoundToInt(level * 0.4f);
		ammoSize = ammoCount;
		reserveAmmo = ammoSize * 5;
		reloadTime -= Mathf.Clamp((level * 0.15f), 0.5f, 10f);

		if (gunClass == Class.Shotgun) {
			pelletCount += Mathf.RoundToInt(level * 0.3f);
		}
	}

	void DetermineModifiers() {
		float chanceForMods = Random.Range (0, 100);
		if (chanceForMods >= 40) {
			firstMod = (Modifier)Random.Range (1, 6);
			if (chanceForMods >= 80) {
				secondMod = (Modifier)Random.Range (1, 6);
			}
			ApplyModifiers ();
		}
	}

	void ApplyModifiers(){
		switch (firstMod) {
		case (Modifier.None):
			break;
		case (Modifier.DamagePlus):
			damage = (damage * 1.2f) + 6f;
			break;
		case (Modifier.FireRatePlus):
			fireRate *= 1.3f;
			break;
		case (Modifier.MagPlus):
			ammoCount = Mathf.RoundToInt(ammoCount * 1.2f);
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			break;
		case (Modifier.ReloadPlus):
			reloadTime *= 0.75f;
			break;
		case (Modifier.PelletPlus):
			pelletCount++;
			accuracy += 0.07f;
			break;
		}

		switch (secondMod) {
		case (Modifier.None):
			break;
		case (Modifier.DamagePlus):
			damage = (damage * 1.2f) + 6f;
			break;
		case (Modifier.FireRatePlus):
			fireRate *= 1.3f;
			break;
		case (Modifier.MagPlus):
			ammoCount = Mathf.RoundToInt(ammoCount * 1.2f);
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			break;
		case (Modifier.ReloadPlus):
			reloadTime *= 0.75f;
			break;
		case (Modifier.PelletPlus):
			pelletCount++;
			accuracy += 0.07f;
			break;
		}
	}

	void DetermineRarity() {
		float rarityRoll = Random.Range (0, 100);
		if (rarityRoll >= 98) {
			rarity = Rarity.Legendary;
		} else if (rarityRoll >= 95) {
			rarity = Rarity.Epic;
		} else if (rarityRoll >= 80) {
			rarity = Rarity.Rare;
		} else if (rarityRoll >= 60) {
			rarity = Rarity.Uncommon;
		} else {
			rarity = Rarity.Common;
		}
		ApplyRarity ();
	}

	void ApplyRarity() {
		switch (rarity) {
		case (Rarity.Common):
			break;
		case (Rarity.Uncommon):
			damage *= 1.1f;
			reloadTime *= 0.95f;
			break;
		case (Rarity.Rare):
			damage *= 1.2f;
			fireRate *= 1.1f;
			ammoCount = Mathf.RoundToInt(ammoCount * 1.1f);
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime *= 0.9f;
			break;
		case (Rarity.Epic):
			damage *= 1.3f;
			fireRate *= 1.2f;
			ammoCount = Mathf.RoundToInt(ammoCount * 1.2f);
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime *= 0.85f;
			break;
		case (Rarity.Legendary):
			damage *= 1.5f;
			fireRate *= 1.25f;
			pelletCount++;
			ammoCount = Mathf.RoundToInt(ammoCount * 1.5f);
			ammoSize = ammoCount;
			reserveAmmo = ammoSize * 5;
			reloadTime *= 0.8f;
			break;
		}
	}

	//disallow extreme possibly game-breaking values such as negative reload speed 
	void Clamping() {
		damage = Mathf.Round (damage);
		fireRate = Mathf.Clamp (fireRate, 1f, 20f);
		fireRate = Mathf.Round (fireRate * 10.0f) / 10.0f;
		reloadTime = Mathf.Clamp (reloadTime, 0.5f, 99f);
		accuracy = Mathf.Clamp (accuracy, 0f, 0.3f);
	}

	public string GetRarity() {
		return rarity.ToString ();
	}
}
