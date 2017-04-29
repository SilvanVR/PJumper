using UnityEngine;
using System.Collections;

/*
 * This Script controls the Collision between the Player and the CircleCollider from the Space-Object itself
 */

public class SpaceObjectCollider : MonoBehaviour {

	//Called when a Rigidbody hits the Collider
	void OnCollisionEnter2D (Collision2D other){	
		if (other.gameObject.tag == "Player"){	
			Player_Movement player_mov = other.gameObject.GetComponent<Player_Movement>();
			player_mov.setGroundedTrue (this.gameObject);
			if (this.gameObject.CompareTag ("Earth")) GameManager.instance.levelComplete ();
		}
	}
}
