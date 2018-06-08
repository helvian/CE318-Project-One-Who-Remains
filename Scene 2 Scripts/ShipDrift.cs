using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Moves the enemy boss ship back and forth
 */

public class ShipDrift : MonoBehaviour {

	public Vector3 rightAnchor; //point on right to move to
	public Vector3 leftAnchor; //point on left to move to
	public float offset; //how far from anchors to move to
	public bool movingLeft = false;
	public float speed = 0.02f;

	void FixedUpdate () {
		//move left gradually faster until past the desired spot
		if (movingLeft) {
			transform.position = Vector3.Lerp (transform.position, leftAnchor, speed * Time.deltaTime);
			if (Vector3.Distance (transform.position, leftAnchor) < offset) {
				movingLeft = false;
				speed = 0.02f;
			}
			speed += 0.01f;
		} 
		//move right gradually faster until past the desired spot
		else if (!movingLeft) {
			transform.position = Vector3.Lerp (transform.position, rightAnchor, speed * Time.deltaTime);
			if (Vector3.Distance (transform.position, rightAnchor) < offset) {
				movingLeft = true;
				speed = 0.02f;
			}
			speed += 0.01f;
		}

	}

}
