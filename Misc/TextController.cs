using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Class that handles all text updates
 */

public class TextController : MonoBehaviour {

	//ammo texts
	public Text ammoText;
	public Text reserveAmmoText;

	//health texts
	public Text healthText;
	public Text armourText;

	//player bars
	public Slider healthBar;
	public Slider armourBar;

	//money text
	public Text moneyText;

	//health texts for scene 2
	public Text helicopterHealthText;
	public Text bossHealthText;

	//wave text
	public Text waveText;

	//text for weapon stats
	public Text[] weaponTexts;

	//context sensitive text
	public Text contextActionText;

	public void updateAmmo(int ammo) {
		ammoText.text = ammo.ToString ();
	}

	public void updateReserveAmmo(int reserveAmmo){
		reserveAmmoText.text = reserveAmmo.ToString ();
	}

	public void updateHealth(int health){
		healthText.text = health.ToString ();
		healthBar.value = health;
	}

	public void updateArmour(int armour){
		armourText.text = armour.ToString ();
		armourBar.value = armour;
	}

	public void updateMoney(int money){
		moneyText.text = money.ToString ();
	}

	public void updateWave(int wave){
		waveText.text = "Wave: " + wave.ToString ();
	}

	public void updateWeaponStats(Gun gun) {
		weaponTexts [0].text = Mathf.RoundToInt (gun.damage).ToString () + "x" + gun.pelletCount.ToString();
		weaponTexts [1].text = (100 - (gun.accuracy *100) ).ToString ();
		weaponTexts [2].text = gun.fireRate.ToString ();
		weaponTexts [3].text = gun.ammoSize.ToString ();
		weaponTexts [4].text = gun.reloadTime.ToString ();
		weaponTexts [5].text = gun.GetRarity ();
	}

	public void showContextText(Gun gun, int price) {
		contextActionText.enabled = true;
		contextActionText.text = "Press F to buy " + gun.gunClass + " for " + price.ToString ();
	}

	public void showEndGameText(int price) {
		contextActionText.enabled = true;
		contextActionText.text = "Press F to escape for " + price.ToString ();
	}

	public void hideContextText() {
		contextActionText.enabled = false;
		contextActionText.text = "";
	}

	public void InsufficientFundsText() {
		contextActionText.enabled = true;
		contextActionText.text = "You don't have enough money!";
	}

	public void updateHelicopterHealth(int health){
		helicopterHealthText.text = "Helicopter Health: " + health.ToString ();
	}

	public void updateBossHealth(float health){
		bossHealthText.text = "Boss Health: " + Mathf.Round(health).ToString();
	}

}
