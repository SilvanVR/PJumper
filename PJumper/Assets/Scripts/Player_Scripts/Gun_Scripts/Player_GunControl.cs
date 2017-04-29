using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_GunControl : MonoBehaviour {

	public Rigidbody2D gunshoot;
	public KeyCode shoot;
	public float fireRate = 2;
	public int speed = 10;
	public int damage = 10;
	public bool hasGun = false;
	
	public UI_Button uI_shootButton;
	public Button buyGunButton;
	public GameObject touchControl;

	private float timeToFire;
	private Transform spawn;
	private bool shootPossible = true;

	void Awake (){
		spawn = transform.FindChild ("FireSpawn"); //The Spawn-Point for the Bullets
	}

	// Update is called once per frame
	void Update () {
		if (!hasGun || !shootPossible) return; //Player has no Gun, return then

		if ((Input.GetKey (shoot) || uI_shootButton.isPressed) && Time.time > timeToFire) { //Fire-Button pressed and timeToFire is reached?
			timeToFire = Time.time + 1 / fireRate; //Set timeToFire new
			Rigidbody2D instance = Instantiate (gunshoot,spawn.position,transform.rotation) as Rigidbody2D; //Instantiate an bullet
			if (transform.localScale.x > 0) instance.velocity = transform.right * speed;	//Set the Velocity of the Bullet (right)
			else instance.velocity = -transform.right * speed;								//Set the Velocity of the Bullet (left)
		}
	}

	/*
	 * Activates/Deactivates the Gun from the Player.
	 * Called from. Player_Movement (T-Button) FOR TESTING - REMOVE LATER
	 */
	public void triggerGun (){
		if (hasGun) {			
			removeGun ();
		}else{
			addGun ();
		}
	}

	/*
	 * Adds/Removes the Gun.
	 * Called from: Player_Inventory-Script
	 */
	public void addGun (){
		buyGunButton.interactable = false;
		hasGun = true;		
		shootPossible = true;
		GameManager.setEnemyLifeBars (true); 			//Actives the Enemy-Lifebars
		GetComponent<Animator> ().SetTrigger ("GetGun");
		if (touchControl.activeSelf) uI_shootButton.gameObject.SetActive (true);		//Activate UI-Button
	}
	public void removeGun (){
		buyGunButton.interactable = true;
		hasGun = false;		
		GameManager.setEnemyLifeBars (false);			//Deactives the Enemy-Lifebars
		GetComponent<Animator> ().SetTrigger ("DropGun");
		uI_shootButton.gameObject.SetActive (false);	//Deactivate UI-Button
	}

	/* 
	 * Deactivates the ability to Shoot
	 * Called from: Player_Movement (burnToDeath () ) and Player_TimeInSpace (dieInSpace () )
	 */
	public void deactiveGun (){shootPossible = false;}
	public void activateGun (){shootPossible = true;} //Called from Player_Checkpoints

	public int getDamage (){return damage;} //Get the Amount of damage the Weapon will do - Called from the Bullets itself

}
