using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Mutant AI using Seek steering behaviour and controlling animations and attacking
 */

public class MutantBehaviour : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent nma;
	public GameObject player;
	public Animator animator; 
	public Collider meleeAttack; //collider box showing the area an attack will hit
	public AudioSource footsteps;

	void Start () {
		nma = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		nma.speed = GetComponent<Mutant> ().moveSpeed; //set speed to defined speed in Mutant
		player = GameObject.FindGameObjectWithTag ("Player");
		meleeAttack = transform.FindChild ("Attack Box").GetComponent<Collider>();
		footsteps = GetComponent<AudioSource> ();
		StartCoroutine (Seek ());
		StartCoroutine (Footsteps ());
	}

	void Update() {
		float speed = 0;
		DetermineAnimParameters (out speed);
		animator.SetFloat ("Speed", speed, 0.1f, Time.deltaTime);
	}

	//determine whether mutant should be running, walking, or standing still
	void DetermineAnimParameters(out float speed) {
		speed = Vector3.Magnitude (Vector3.Project (nma.desiredVelocity, Vector3.forward));
	}

	//chase the enemy while not dead
	IEnumerator Seek() {
		while (animator.GetBool("Dead") == false) {

			//attack if close enough
			if ((Vector3.Distance (transform.position, player.transform.position)) < 2.5f) {
				animator.SetBool ("Attack", true);
				yield return new WaitForSeconds (0.75f);
				meleeAttack.enabled = true;
				yield return new WaitForSeconds (1f);
				meleeAttack.enabled = false;
				animator.SetBool ("Attack", false);
			} 
			//otherwise keep chasing
			else {
				nma.SetDestination (player.transform.position);
				yield return new WaitForEndOfFrame ();
			}
		}
		//death
		nma.isStopped = true;
		yield return new WaitForSeconds (0.6f);
		Destroy (gameObject);
	}

	//play footstep noises while the mutant is moving and not attacking
	IEnumerator Footsteps() {
		while (true) {
			if (animator.GetFloat ("Speed") > 2f && animator.GetBool("Attack") == false && footsteps.isPlaying == false) {
				footsteps.pitch = Random.Range (0.7f, 1.1f);
				footsteps.Play ();
			}
			yield return new WaitForEndOfFrame();
		}

	}
		
}
