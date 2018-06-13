using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Script controlling the shooting behaviour and gun transform manipulation
 */

public class GunController : MonoBehaviour {

	public ParticleSystem muzzleFlash; //effect from the barrel
	public ParticleSystem spray; //effect on the spot hit by gun
	public Camera cam; //camera viewpoint of the player
	private int layerMask; 
	public GameObject particleParent; //cleanup object for particles
	public GameObject statObjectParent; //secondary storage for gun stats

	public TextController tc;
	public AudioSource au;
	public Inventory inv;
	public WeaponSwitcher ws;
	public Vector3 restingRotation; //rotation to return to when gun is not kicking or reloading

	private float nextTimeToFire = 0f;

	private bool infiniteAmmo = false; 
	public bool reloading;

	public Gun gun; //gun stats in the held gun

	public int sceneIndex;

	void Awake() {
		sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		if (sceneIndex == 2) {
			SceneManager.sceneLoaded += OnLevelLoad;
		}
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController>();
		DontDestroyOnLoad (transform.gameObject);
		int powerUpLayerIndex = LayerMask.NameToLayer("PowerUp");
		layerMask = ~(1 << powerUpLayerIndex);
		inv = GetComponentInParent<Inventory> ();
		particleParent = GameObject.FindGameObjectWithTag ("Particle Parent");
		statObjectParent = GameObject.FindGameObjectWithTag ("Stat Object");
		reloading = false;
		restingRotation = transform.localRotation.eulerAngles;
	}

	void Start() {
		infiniteAmmo = false;
		tc.updateAmmo (gun.ammoCount);
		tc.updateReserveAmmo (gun.reserveAmmo);
	}

	//re-read inventory and reassign objects on scene transition
	void OnLevelLoad(Scene scene, LoadSceneMode mode) {
		if (sceneIndex == 2) {
			inv = GetComponentInParent<Inventory> ();
			tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
			particleParent = GameObject.FindGameObjectWithTag ("Particle Parent");
			reloading = false;
			infiniteAmmo = true;
			restingRotation = transform.localRotation.eulerAngles;
		}
	}

	//detect fire inputs and reloading inputs
	void Update () {
		//if there is ammo and not reloading and player tries to shoot
		if (gun.ammoCount > 0 && !reloading) {
			if (Input.GetButton ("Fire1") && Time.time >= nextTimeToFire) {
				nextTimeToFire = Time.time + 1f / gun.fireRate;
				Shoot ();
				gun.ammoCount--;
				tc.updateAmmo (gun.ammoCount);
			}
		} 
		//if there is reserve ammo left and the gun is empty and not already reloading
		else if (gun.ammoCount <= 0 && gun.reserveAmmo > 0 && !reloading) {
			StartCoroutine (Reload ());
		}
		//if playing reloads early and is not already reloading and has reserve ammo left
		if (Input.GetButton ("Fire2") && !reloading && gun.reserveAmmo > 0) {
			StartCoroutine (Reload ());
		}
		//if not reloading, reset the gun to resting position
		if (!reloading) {
			transform.localRotation = Quaternion.Slerp (transform.localRotation, Quaternion.Euler (restingRotation), Time.deltaTime);
		}
	}

	//read stats of weapon to the weapon that is switched to 
	void OnEnable() {
		LoadGunStats (ws.chosenWeapon);
	}

	//get the Nth stat object according to gunIndex and read stats into the gun
	void LoadGunStats (int gunIndex) {
		GameObject statObject = statObjectParent.transform.FindChild ("Slot " + gunIndex).gameObject;
		gun = statObject.GetComponent<Gun> ();
		tc.updateAmmo (gun.ammoCount);
		tc.updateReserveAmmo (gun.reserveAmmo);
	}

	//reload weapon
	IEnumerator Reload() {
		ReloadAnimation ();
		reloading = true; //prevent further reload invocation
		yield return new WaitForSeconds (gun.reloadTime);
		if (!infiniteAmmo) {
			//if the player doesnt have enough ammo to fill an entire mag
			if (gun.reserveAmmo < gun.ammoSize) {
				gun.ammoCount += gun.reserveAmmo;
				gun.reserveAmmo = 0;
			} 
			//if the player reloads from an empty mag with enough ammo
			else if (gun.ammoCount == 0) {
				gun.ammoCount += gun.ammoSize;
				gun.reserveAmmo -= gun.ammoSize;
			} 
			//if the player reloads early
			//6 ammo, 8 ammo size, 1 reserve ammo - difference is 2
			else {
				int difference = gun.ammoSize - gun.ammoCount;
				//if the player does not have enough ammo to fill the difference
				if (difference > gun.reserveAmmo) {
					gun.ammoCount += gun.reserveAmmo;
					gun.reserveAmmo = 0;
				} 
			//if the player has enough ammo to fill the difference
			else {
					gun.ammoCount += difference;
					gun.reserveAmmo -= difference;
				}
			} 
		} 
		//refill ammo if infinite ammo is active
		else {
			gun.ammoCount += gun.ammoSize;
		}
		tc.updateReserveAmmo (gun.reserveAmmo);
		tc.updateAmmo (gun.ammoCount);
		reloading = false;
	}

	//shooting
	void Shoot() {
		RaycastHit hit;
		au.PlayOneShot(au.clip);
		GunKick (); 

		//for each bullet to be fired per click
		for (int i = 0; i < gun.pelletCount; i++) {
			//displace the ray direction by a certain amount
			Vector3 bulletSpread = cam.transform.forward + (Random.insideUnitSphere * gun.accuracy);
			muzzleFlash.Play();
			Ray ray = new Ray (cam.transform.position, bulletSpread);
			if (Physics.Raycast(ray, out hit, gun.range, layerMask)) {
				//spawn a laser hit effect at the place of impact
				ParticleSystem spawnedSpray = Instantiate (spray, hit.point, Quaternion.identity) as ParticleSystem;
				spawnedSpray.transform.parent = particleParent.transform;

				//check if any of the following enemies were hit and damage them if they were
				Drone d = hit.transform.GetComponent<Drone> ();
				if (d != null) {
					d.TakeDamage (Mathf.Round (Random.Range (gun.damage * 0.8f, gun.damage * 1.2f)));
					inv.money += 10;
				} 
				Mutant m = hit.transform.GetComponent<Mutant> ();
				if (m != null) {
					m.TakeDamage (Mathf.Round (Random.Range (gun.damage * 0.8f, gun.damage * 1.2f)));
					inv.money += 10;
				}
				BossParts bp = hit.transform.GetComponent<BossParts> ();
				if (bp != null) {
					bp.TakeDamage (Mathf.Round (Random.Range (gun.damage * 0.8f, gun.damage * 1.2f)));
				}
				MissileSeek mi = hit.transform.GetComponent<MissileSeek> ();
				if (mi != null) {
					mi.TakeDamage (Mathf.Round (Random.Range (gun.damage * 0.8f, gun.damage * 1.2f)));
				}

			}
		}
		tc.updateMoney (inv.money);
	}

	//kick the gun model upwards 
	//values are different due to the native (0, 0, 0) rotation being different for every gun model
	void GunKick() {
		switch (gun.gunClass) {
		case (Gun.Class.Pistol):
			transform.Rotate (transform.forward * -4, Space.World);
			break;
		case (Gun.Class.AssaultRifle):
			transform.Rotate (transform.right * -2, Space.World);
			break;
		case (Gun.Class.SMG):
			transform.Rotate (transform.right * -1, Space.World);
			break;
		case (Gun.Class.Shotgun):
			transform.Rotate (transform.forward * 5, Space.World);
			break;
		case (Gun.Class.Sniper):
			transform.Rotate (transform.forward * 10, Space.World);
			break;
		case (Gun.Class.LMG):
			transform.Rotate (transform.right * -2, Space.World);
			break;
		}
	}

	//lower the gun model on reload 
	void ReloadAnimation() {
		switch (gun.gunClass) {
		case (Gun.Class.Pistol):
			transform.Rotate (transform.forward * 30, Space.World);
			break;
		case (Gun.Class.AssaultRifle):
			transform.Rotate (transform.right * 40, Space.World);
			break;
		case (Gun.Class.SMG):
			transform.Rotate (transform.right * 40, Space.World);
			break;
		case (Gun.Class.Shotgun):
			transform.Rotate (transform.forward * -40, Space.World);
			break;
		case (Gun.Class.Sniper):
			transform.Rotate (transform.forward * -40, Space.World);
			break;
		case (Gun.Class.LMG):
			transform.Rotate (transform.right * 40, Space.World);
			break;
		}
	}}
