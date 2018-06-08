using UnityEngine;
using System.Collections;

/*
 * Script controlling the mouse looking behaviour 
 * Code built upon the base found in holistic3d's tutorial:
 * https://www.youtube.com/watch?v=blO039OzUZc
 */

public class CamMouseControl : MonoBehaviour {

	private Vector2 mouseLook;
	private Vector2 smoothVector;
	public float sens = 4.0f;
	public float smoothing = 2.0f;
	private GameObject player;
	private Cursor c;

	void Start () {
		player = this.transform.parent.gameObject;
	}
	
	void Update () {
		//make sure cursor is locked in the center
		Cursor.lockState = CursorLockMode.Locked;

		//get movement of mouse this frame and scale it up
		Vector2 direction = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		direction = Vector2.Scale (direction, new Vector2 (sens * smoothing, sens * smoothing));
		smoothVector.x = Mathf.Lerp (smoothVector.x, direction.x, 1f / smoothing);
		smoothVector.y = Mathf.Lerp (smoothVector.y, direction.y, 1f / smoothing);

		//move the camera with the created movement vector then rotate the camera and player accordingly
		mouseLook += smoothVector;
		mouseLook.y = Mathf.Clamp (mouseLook.y, -90f, 90f);
		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
		player.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, player.transform.up);
	}
}
