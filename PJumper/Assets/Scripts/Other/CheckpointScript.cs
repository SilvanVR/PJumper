using UnityEngine;
using System.Collections;

public class CheckpointScript : MonoBehaviour {

	public bool showText = true;

	private bool calledOnce = false;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (calledOnce) return;
		if (other.CompareTag ("Player")) {
			other.gameObject.GetComponent<Player_Checkpoints> ().setCurrentCheckpoint (this.transform,showText);		
			calledOnce = true;
		}
	}

}
