using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Drone AI using Arrival steering behaviour and controlling particle effects
 */

public class DroneBehaviour : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent nma;
	public GameObject player;
	public float arrivalSlowingDist; //how far it must be to start slowing down
	public float speed;
	public float nextFire = 0f; 
	public float fireRate = 1f;

	public ParticleSystem spray; //effect made when hitting player
	public ParticleSystem explosion; //effect made on death
	public AudioSource explosionSound; //sound made on death
	public GameObject particleParent; //object that tidies up spawned particles
	public Drone d; //reference to the drone object

	void Start () {
		particleParent = GameObject.FindGameObjectWithTag ("Particle Parent");
		nma = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		speed = GetComponent<Drone> ().moveSpeed;
		nma.speed = GetComponent<Drone> ().moveSpeed;
		player = GameObject.FindGameObjectWithTag ("Player");
		d = GetComponent<Drone> ();
		StartCoroutine (Arrival ());
	}

	//steering behaviour
	IEnumerator Arrival() {
		while (!d.dead) {
			Vector3 target = player.transform.position;
			nma.SetDestination (target);
			Vector3 targetOffset = (target - transform.position);

			//shooting if close enough
			if ((Vector3.Distance (transform.position, target)) < arrivalSlowingDist * 1.5f) {
				if (Time.time > nextFire) {
					nextFire = Time.time + fireRate;
					RaycastHit hit;
					Ray ray = new Ray (transform.position, targetOffset);

					//if the ray hit the player, damage them and spawn a hit effect
					if (Physics.Raycast (ray, out hit)) {
						PlayerController p = hit.transform.GetComponent<PlayerController> ();
						ParticleSystem spawnedSpray = Instantiate (spray, hit.point, Quaternion.identity) as ParticleSystem;
						spawnedSpray.transform.parent = particleParent.transform;
						if (p != null) {
							p.TakeDamage (d.damage, "Drone");
						}
					}
				}
			}

			//moving if not close enough, slowing down if in stopping range
			transform.forward = targetOffset;
			float distance = targetOffset.magnitude - nma.stoppingDistance;
			float rampedSpeed = nma.speed * (distance / arrivalSlowingDist);
			float reducedSpeed = Mathf.Min (speed, rampedSpeed);
			nma.speed = reducedSpeed;
			yield return new WaitForEndOfFrame ();
		}

		//death and explosion spawning
		nma.isStopped = true;
		ParticleSystem spawnedExplosion = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
		spawnedExplosion.transform.parent = particleParent.transform;
		spawnedExplosion.Play ();
		d.enabled = false;
		explosionSound.Play ();
		yield return new WaitForSeconds (0.6f);
		Destroy (gameObject);
	}

}
