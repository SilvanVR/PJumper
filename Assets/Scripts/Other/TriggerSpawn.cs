using UnityEngine;
using System.Collections;

public class TriggerSpawn : MonoBehaviour {

	public GameObject[] spawnObjects;
	public bool callOnce = false;

	private bool calledOnce;

	void OnTriggerEnter2D (Collider2D other){
		if (other.CompareTag ("Player")){			
			if (calledOnce) return;
			for (int i = 0; i < spawnObjects.Length; i++) {
				if (spawnObjects[i].CompareTag ("Asteroid")) { //Respawns an Asteroid if it is Active
					if (spawnObjects[i].activeSelf) spawnObjects[i].GetComponent<Asteroid_Movement> ().respawn ();		
				}
				spawnObjects[i].SetActive (true);
			}
			if (callOnce) calledOnce = true;
		}
	}
}
