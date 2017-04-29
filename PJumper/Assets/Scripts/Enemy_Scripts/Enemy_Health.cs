using UnityEngine;
using System.Collections;

/*
 * This Script controls the Life of an Enemy
 */
public class Enemy_Health : MonoBehaviour {

	public float maxhealth = 100; //max Health
	public float curhealth;	//current Health
	public Transform healthbar; //Child-Transform Component from "healthbar"
	public GameObject blood;	//Particle System (Blood-Effect)

	public GameObject spawnObject;

	private float startScale; //Holds the Startscale from the Healthbar

	void Awake (){
		curhealth = maxhealth;						//Set current Health to max Health
		startScale = healthbar.localScale.x;		//Set the Start - Scale from the healthbar
	}

	/*
	 * Applies the given damage to the health-value.
	 * Recalculate the Scale of the Child-Object "healthbar".
	 * Instantiate the "Blood" Prefab
	 * Called from: Gun_Movement-Script (The Bullets itself)
	 */
	public void applyDamage (int damage,Vector2 hitPos){
		curhealth -= damage;	
		Instantiate (blood, hitPos, Quaternion.identity);
		healthbar.transform.localScale = new Vector3 (curhealth / maxhealth * startScale,1,1); //Recalc Scale
		if (curhealth <= 0) {	//Enemy is dead
			kill ();	
		}
	}

	/*
	 * Kills the Enemy and removes it from the Game.
	 */
	private void kill (){
		if (spawnObject != null) spawnObject.SetActive (true);
		Destroy (this.gameObject);
	}
}
