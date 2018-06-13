using UnityEngine;
using System.Collections;

/*
 * Script used to destroy effects after they are done
 */

public class DestroyEffect : MonoBehaviour {

	void Start () {
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		Destroy (gameObject, ps.main.duration *2);
	}

}
