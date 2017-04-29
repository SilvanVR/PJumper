using UnityEngine;
using System.Collections;

public class Sun_CircleCollider : MonoBehaviour {

	//Called when a Rigidbody hits the Collider
	void OnCollisionEnter2D (Collision2D other){	
		if (other.gameObject.tag == "Player"){	
			other.gameObject.GetComponent<Player_Health>().burnToDeath ();
			other.gameObject.GetComponent<Player_Movement>().setCurrentSpaceObject (this.gameObject);			                                                    
			other.gameObject.transform.SetParent (this.transform);
		}
	}

}
