using UnityEngine;
using System.Collections;

/*
 * This Script set's the Enemy in the "Attack-State". 
 * The Attached 2D-Circle Collider detects an Player.
 */
public class Enemy_AI_Collider : MonoBehaviour {

	private Animator anim;

	void Start () {
		anim = transform.parent.gameObject.GetComponent<Animator> ();
	}
	
	void OnTriggerStay2D (Collider2D other){
		if (other.CompareTag ("Player")) {
			attack ();
		}
	}

	private void attack (){
		anim.SetTrigger ("Attack");
	}
}
