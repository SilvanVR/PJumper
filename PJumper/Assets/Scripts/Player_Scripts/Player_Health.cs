using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * This Script controls the Life from the Player.
 */
public class Player_Health : MonoBehaviour {

	public int lifes = 3;	

	public float maxhealth = 100; //Max Health
	public float curhealth;		//current Health

	private Transform healthbar; //Child-Transform Component from "healthbar"
	private float startScale; //Holds the Startscale
	private Text uI_curLifes; //The Text-Component for displaying the number of lifes


	void Start (){
		healthbar = transform.GetChild (0).transform; //Get Child's Transform-Component
		curhealth = maxhealth;						//Set current Health to max Health
		startScale = healthbar.localScale.x;		//Set the Start - Scale
	}

	/*
	 * Applies the given damage to the health-value.
	 * Called from: Gun_Movement-Script (The Bullets itselfs).
	 */
	public void applyDamage (int damage){
		curhealth -= damage;
		healthbar.transform.localScale = new Vector3 (curhealth / maxhealth * startScale,1,1); //Recalc Scale
		if (curhealth <= 0) {
			kill ();		
		}
	}
	
	/*
	 * Called from the burning-Animation at the End.
	 * Called from the Player_TimeInSpace-Script.
	 */
	public void kill (){		
		lifes--;		
		updateUI ();
		if (lifes > 0) {
			respawn ();
		}else{
			this.gameObject.SetActive (false);
			GameManager.instance.removePlayer (this.gameObject); //Notify the Manager (Restarts the Game if numPlayers = 0)
		}
	}
		
	/*Called from a Sun, kills the Player*/
	public void burnToDeath(){
		GetComponent<BoxCollider2D> ().enabled = false;
		GetComponent<Player_Movement> ().stopMove (); //Stop Player from Moving
		GetComponent<Player_GunControl> ().deactiveGun (); //Deactivate Gun if Player has one
		GetComponent<Animator> ().SetTrigger ("Burning");	//Start burning-Animation ---> Calls kill () at the End
	}

	//Set's a reference to the UI-Component, called from Player_UI_SlotControl
	public void setUIText (Text comp){
		uI_curLifes = comp;
		updateUI ();
	}

	//add lifes to the Player - Called from the Shop-Script
	public void addLifes (int lifes){
		this.lifes += lifes;
		updateUI ();
	}

	//------------------------------------------ PRIVATE METHODS ---------------------------------------------------
	private void respawn (){
		GetComponent<Player_Checkpoints> ().respawnAtLastCheckpoint ();		
	}

	private void updateUI (){
		uI_curLifes.text = "" + lifes;
	}

}
