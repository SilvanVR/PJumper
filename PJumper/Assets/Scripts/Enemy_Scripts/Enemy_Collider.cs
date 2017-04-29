using UnityEngine;
using System.Collections;

/*
 * This Script controls the Collision between the Player and the Enemys.
 */

public class Enemy_Collider : MonoBehaviour {

	public int damage = 100;

	//Called when a Rigidbody hits the Collider
	void OnCollisionEnter2D (Collision2D other){	
		if (other.gameObject.tag == "Player"){	
			other.gameObject.GetComponent<Player_Health>().applyDamage (damage);
		}
	}

}
