using UnityEngine;
using System.Collections;

/*
 * Constantly rotates an object based on given parameters
 */

public class PickUpSpinner : MonoBehaviour {

	public float spinSpeed;
	public float tiltSpeed;

	void Update () {
		transform.Rotate (Vector3.right * tiltSpeed * Time.deltaTime);
		transform.Rotate (Vector3.up * spinSpeed * Time.deltaTime);
	}
}
