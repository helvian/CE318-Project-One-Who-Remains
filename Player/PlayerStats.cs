using UnityEngine;
using System.Collections;

/*
 * Variables for the player
 */
public class PlayerStats : MonoBehaviour {

	public int health = 200;
	public int armour = 0;

	public int maxHealth = 200;
	public int maxArmour = 100;

	public float speed = 5f; //how fast the player moves
	public float gravity = 600f; //how fast the player falls
}
