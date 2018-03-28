using UnityEngine;
using System.Collections;

/*
 * This Script conrols the Movement of the Asteroid and when he have to Respawn (based on MaxRange) - Includes the Respawn - Animation
 */
public class Asteroid_Movement : MonoBehaviour {

	public Vector2 direction;			//The Direction Vector
	public float maxRange = 15;			//The Max-Range the Asteroid can move until he will respawn
	public float scaleDownSpeed = 0.95f; //The Scale-Down-Value (Multiplied every Frame with local Scale)
	public GameObject star;				//Star Prefab
	public float range = 0;				//The current Range
	public float speedMult = 1;

	private Vector2 spawnPoint;
	private Vector2 oldDirection;
	private Vector3 oldScale;
	private bool calledOnce = false;
	private bool releasedPlayerFromMe = false;

	// Use this for initialization
	void Start () {
		direction *= 0.1f;
		spawnPoint = transform.position;	//Sets the Spawn-Point to the Current-Position
		oldDirection = direction;			//Save the Direction-Vector 
		oldScale = transform.localScale;	//Save the Old-Scale
	}

	void FixedUpdate () {		
		
		direction = oldDirection * speedMult;
		if (calledOnce) return;			//Calculate Nothing if Asteroid begins to Respawn
				
		transform.position += (Vector3)direction;			//Adds the direction-Vector to the Position
		range += direction.magnitude;					//Calculate the covered Range
		if (range > maxRange) {			//Covered Range bigger than maxRange?

			if (checkIfPlayerIsOnMe() && !releasedPlayerFromMe){
				setPlayerParent();
				releasedPlayerFromMe = true;
			}
			if (scaleAsteroidDown ()){ //Scale Asteroid down, returns True if Scale = 0
				calledOnce = true;	   
				ReleasePlayerIfHeIsOnMe (); //Kill Player if Asteroid begins to Respawn
				StartCoroutine(destroyAsteroid ()); //Instantiate the Star-Prefab + Respawn after it
			}
		}
	}

	/*Called from the Asteroid_CircleCollider Script to add this dir-Vector to Players-Transform-Component*/
	private Vector2 getDirectionVector (){return direction;} 

	/*Scale the Asteroid down until Scale is 0, returns true then*/
	private bool scaleAsteroidDown (){
		if (transform.localScale.x == 0) return true;
		else{		
			transform.localScale = new Vector3 (transform.localScale.x * scaleDownSpeed, transform.localScale.y * scaleDownSpeed,1);
			if (Mathf.Abs (transform.localScale.x) < 0.1f) transform.localScale = Vector3.zero;			
			return false;
		}
	}

	/*Called from this Script if range > maxrange and scale = 0*/
	private IEnumerator destroyAsteroid (){
		Instantiate (star, transform.position, Quaternion.identity);
		GetComponent<CircleCollider2D> ().enabled = false; //Disable the Hit-Collider from the Asteroid
		transform.Find ("Gravity_Zone").GetComponent<CircleCollider2D> ().enabled = false; //Disable the Trigger-Collider from the Gravity-Zone
		yield return new WaitForSeconds (3);
		respawn ();
	}

	//Set the Player's Parent to the "PlayerParent-GameObject" if Asteroid begins to Respawn (Otherwise it scales the Player too)
	private void setPlayerParent (){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null) return;
		if (checkIfPlayerIsOnMe ()){
			player.transform.SetParent (GameObject.Find("PlayerParent").transform);
		}
	}

	//Notify the Player to change the current Player-Parent
	private void ReleasePlayerIfHeIsOnMe(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");		
		if (player == null) return;
		Player_Movement player_mov = player.GetComponent<Player_Movement> ();
		if (player_mov.getCurrentSpaceObject () == this.gameObject && player_mov.grounded){			
			player_mov.releasePlayerFromCurSpaceObject ();
			player_mov.setCurrentSpaceObject (null);
		}
	}

	private bool checkIfPlayerIsOnMe (){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");		
		if (player == null) return false;
		Player_Movement player_mov = player.GetComponent<Player_Movement> ();
		if (player_mov.getCurrentSpaceObject () == this.gameObject && player_mov.grounded){			
			return true;
		}
		return false;
	}
	//--------------------------------------------- PUBLIC ---------------------------------------------------	
	
	//Called from the Script itself and an Blackhole with the "Blackhole_Gravity_Asteroid - Script"
	public void respawn (){
		transform.localScale = oldScale;					//Reset Scale
		range = 0;											//Reset Range
		calledOnce = releasedPlayerFromMe = false;									//Reset calledOnce
		transform.position = spawnPoint;					//Reset Position
		direction = oldDirection;							//Reset the Direction (could be modified if Asteroid collided with, possibly, a Planet)
		GetComponent<CircleCollider2D> ().enabled = true;	//Enable the Hit-Collider from the Asteroid
		transform.Find ("Gravity_Zone").GetComponent<CircleCollider2D> ().enabled = true;	//Enable the Trigger-Collider from the Gravity-Zone
	}

	/*Scale the Player (called from BlackHoles)*/
	public void scale (float scale){
		if (scale < 0.05f) return;
		float locScaleX = transform.localScale.x;
		if (locScaleX > 0) transform.localScale = new Vector2 (scale, scale);
		else transform.localScale = new Vector2 (-scale, scale);
	}	
}
