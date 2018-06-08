using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Rotates helicopter rotors based on given values
 */

public class RotorSpinner : MonoBehaviour {

	public float xSpeed;
	public float ySpeed;
	public float zSpeed;

	void Update() {
		transform.Rotate (Vector3.right * xSpeed * Time.deltaTime, Space.World);
		transform.Rotate (Vector3.up * ySpeed * Time.deltaTime, Space.World);
		transform.Rotate (Vector3.forward * zSpeed * Time.deltaTime, Space.World);
	}
}
