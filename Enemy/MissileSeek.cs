using UnityEngine;
using System.Collections;

/*
 * Missile AI using Seek steering behaviour 
 */

public class MissileSeek : MonoBehaviour {

	public GameObject player;
	public Rigidbody rb;
	public GameObject particleParent; //clean-up parent object for particles

	public Vector3 initialVelocity; //how fast it launches
	public Vector3 currentVelocity; //which way it is going this frame
	public Vector3 desiredVelocity; //which way it wants to go
	public Vector3 desiredDirection; //the difference between the above 2 velocities
	public float speed;

	public float health = 50f;
	public TextController tc;
	public ParticleSystem explosion; //particle made on death

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody> ();
		tc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<TextController> ();
		particleParent = GameObject.FindGameObjectWithTag ("Particle Parent");
		rb.velocity = initialVelocity;
		currentVelocity = rb.velocity;
	}

	//seek steering behaviour - find direction to player then try to turn in that direction
	void seek() {
		desiredVelocity = player.transform.position - rb.transform.position;
		desiredVelocity.Normalize ();
		desiredVelocity *= speed;

		desiredDirection = desiredVelocity - currentVelocity;
	}

	void FixedUpdate () {
		seek ();

		currentVelocity += (desiredDirection / rb.mass);
		currentVelocity.Normalize ();
		currentVelocity *= speed;

		rb.velocity = currentVelocity;

		//face the way it is moving
		transform.up = currentVelocity;
	}

	//reduce health when hit by player, die when health is 0
	public void TakeDamage(float damage) {
		health -= damage;
		if (health <= 0) {
			Death ();
		}
	}

	//play missile explosion noise and spawn death explosion, then destroy self
	public void Death() {
		GameObject.FindGameObjectWithTag ("Death Sound Player").GetComponent<PlayGlobalSound> ()
			.sources [1].Play ();			
		ParticleSystem spawnedExplosion = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
		spawnedExplosion.transform.parent = particleParent.transform;
		spawnedExplosion.Play ();
		Destroy (gameObject);
	}
}
