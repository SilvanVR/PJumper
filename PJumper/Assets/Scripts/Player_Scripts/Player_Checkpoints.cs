using UnityEngine;
using System.Collections;

/*
 * This Script saves the current Checkpoint (Transform-Component) and handles the Respawn if the Player die's.
 */

public class Player_Checkpoints : MonoBehaviour {

	public Transform currentCheckpoint;		//Current Checkpoint
	public Text_Mid textScript;				//Script for displaying a new animated Text on the Screen.

	//Set's a new Checkpoint and display a Animated-Message on the Screen (if showText true)
	public void setCurrentCheckpoint (Transform newCheckpoint, bool showText){
		currentCheckpoint = newCheckpoint;
		if (showText) textScript.newText ("Checkpoint reached!");
	}

	//Reset all necessary variables and respawns the Player.
	public void respawnAtLastCheckpoint (){
		GetComponent<Player_Movement> ().resetScript ();
		GetComponent<Player_TimeInSpace> ().resetScript ();
		GetComponent<BoxCollider2D> ().enabled = true;
		bool hasGun = GetComponent<Player_GunControl> ().hasGun;
		if (hasGun) {
			GetComponent<Animator> ().SetTrigger ("GetGun");
			GetComponent<Player_GunControl> ().activateGun ();
		}else{			
			GetComponent<Animator> ().SetTrigger ("DropGun");
		}
		transform.localScale = Vector3.one;
		transform.position = currentCheckpoint.position;
	}
}
