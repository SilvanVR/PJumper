using UnityEngine;
using System.Collections;

/*
 * This Script controls the Position of the Camera. Mainly it follows the Player.
 * It change the target if the Player dies and they are other Players left.
 */
public class Camera_Follower : MonoBehaviour {

	public Transform target;
	public float stepToTarget = 2;

	void Start (){
		Vector3 pos = new Vector3 (target.position.x,target.position.y,-10);
		transform.position = pos;
	}

	void LateUpdate () {
		transform.position = Vector2.MoveTowards (transform.position, target.position, stepToTarget);
		Vector3 pos = new Vector3 (transform.position.x,transform.position.y,-10);
		transform.position = pos;
	}

	/* 
	 * Called from the GameManager if a Player dies.
	 * Checks if the dead Player was the target.
	 * If not it search a new ACTIVE Player and change the current Target.
	 */
	public void notify (){
		if (target.gameObject.activeSelf) return;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			if (players[i].activeSelf) target = players[i].transform;		
		}
	}

}
