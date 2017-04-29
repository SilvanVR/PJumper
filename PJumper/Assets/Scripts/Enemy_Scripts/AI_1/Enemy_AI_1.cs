using UnityEngine;
using System.Collections;

/*
 * This Script controls the Movement of an Enemy with some "Intelligence".
 * Moves always in the shorter direction to get the Player.
 */

public class Enemy_AI_1 : MonoBehaviour {

	public bool imgFacingRight; //If the Imported Images arent all in the same direction
	public bool movingRight = false;
	public float maxSpeed = 1;
	public GameObject curSpaceObject;	//CurSpaceObject on which the Enemy is on
	public float acceleration;
	
	private bool facingRight = false;
	private float curSpeed = 0; 
	private float halfHeight; 	//Used for Fix-Position
	private Vector2 dir; 		//Direction-Vector. Added every FixedUpdate to transform.position
	private bool isMoving = true;

	void Awake () {
		halfHeight = (GetComponent<BoxCollider2D> ().bounds.size.y / 2); //Used for Fix-Position
		GetComponent<Enemy_Rotation> ().setCurrentSpaceObject (curSpaceObject);
		
		if (imgFacingRight) flipOnce (); //If the Imported Images arent all in the same direction
	}

	void FixedUpdate () {		
		if (curSpaceObject == null || !isMoving) return;
		//TODO Return if Enemy is not on Screen and Player not in current Grav-Radius from curSpaceObject

		detectPlayer ();	//Search an Player and try to catch him as fast as possible
		
		if (!movingRight && facingRight) flip ();
		else if (movingRight && !facingRight) flip();
		
		Vector2 right = getRightVector ();	//The Vector in the right - Direction
		if (movingRight) {	
			curSpeed += acceleration;
			if (curSpeed > maxSpeed) curSpeed = maxSpeed;				
		}else{		
			curSpeed -= acceleration;
			if (curSpeed < -maxSpeed) curSpeed = -maxSpeed;				
		}
		dir = right * curSpeed;

		transform.position += (Vector3) dir;	//Add dir to current Position
		
		fixPosition ();			//Fix the Position
	}

	//----------------------------- PUBLIC METHODS -------------------------------------
	/*Currently not used*/
	public void setCurSpaceObject (GameObject newSpaceObject){
		curSpaceObject = newSpaceObject;
		GetComponent<Enemy_Rotation> ().setCurrentSpaceObject (newSpaceObject);
	}

	//----------------------------- PRIVATE METHODS -------------------------------------
	private void detectPlayer (){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player"); //Search all Players in the Scene

		int index = -1;
		for (int i = 0; i < players.Length; i++) {
			if (players[i].GetComponent<Player_Movement> ().getCurrentSpaceObject () == curSpaceObject){ //TRUE: Player is on same Planet as Enemy
				index = i;
			}		
		}
		if (index < 0) return; //No Player on same Planet found, return then

		float angleEnemy = transform.eulerAngles.z;
		float anglePlayer = players[index].transform.eulerAngles.z;

		bool rightSide;

		float diffRight; //Calculates the right-side distance from Angle to TarAngle
		if(anglePlayer > angleEnemy){
			diffRight = anglePlayer - angleEnemy;
		}else{
			diffRight = 360 - angleEnemy + anglePlayer;
		}						
		if(diffRight < 180) rightSide = false; //Player is nearer in the Left-Direction
		else rightSide = true; //Player is nearer in the Right-Direction

		if (rightSide) movingRight = true;
		else movingRight = false;

	}

	/*Returns a Vector which is a normal-Vector (90 Degree) to the diff-Vector*/
	private Vector2 getRightVector (){
		float diffX = transform.position.x - curSpaceObject.transform.position.x;
		float diffY = transform.position.y - curSpaceObject.transform.position.y;
		float diffBetrag = (float)Mathf.Sqrt(diffX * diffX + diffY * diffY);
		return new Vector2 (diffY / diffBetrag, -diffX / diffBetrag);
	}
	
	/*Flip the Image from the Enemy*/
	private void flip (){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	/*Flip the Image Once*/
	private void flipOnce (){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	/*Fix the Position from the Enemy*/
	private void fixPosition (){		
		Vector3 diff = (transform.position - curSpaceObject.transform.position).normalized;
		transform.position = curSpaceObject.transform.position + diff * (getCurSpaceObjectRadius () + halfHeight + 0.01f); //+0.01f: Set the Enemy a little higher than the collider, so he doesnt trigger always OnColliderEnter 
	}
	
	/*Returns the Radius from the current bounded Space-Object */
	private float getCurSpaceObjectRadius (){
		return curSpaceObject.GetComponent<SpaceObjectDatas> ().radius;
	}
}
