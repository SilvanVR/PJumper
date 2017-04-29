using UnityEngine;
using System.Collections;

public class Blackhole_CircleCollider : MonoBehaviour {

	/*Scale Player to Normal-Size*/
	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "Player"){
			other.gameObject.GetComponent<Player_Movement>().scalePlayer (1f);
		}
	}
}
