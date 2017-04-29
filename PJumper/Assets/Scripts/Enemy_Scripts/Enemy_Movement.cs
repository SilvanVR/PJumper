using UnityEngine;
using System.Collections;

/*
 * This Script controls the Enemy-Movement (Enemy move left or right).
 * It Calculates the Movement based on curSpaceObject. 
 * It Calls "setCurrentSpaceObject" from the "Enemy_Rotation" Script
 */

public class Enemy_Movement : MonoBehaviour {

	public bool imgFacingRight;
	public bool movingRight = false;
	public float maxSpeed = 1;
	public GameObject curSpaceObject;
	private bool isMoving = true;
		
	private bool facingRight = false;
	private float curSpeed;
	private float halfHeight;
	private Vector2 dir;

	// Use this for initialization
	void Awake () {
		halfHeight = (GetComponent<BoxCollider2D> ().bounds.size.y / 2);
		GetComponent<Enemy_Rotation> ().setCurrentSpaceObject (curSpaceObject);

		if (imgFacingRight) flipOnce ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		if (curSpaceObject == null || !isMoving) return;
		//TODO Return if Enemy is not on Screen and Player not in current Grav-Radius from curSpaceObject

		if (!movingRight && facingRight) flip ();
		else if (movingRight && !facingRight) flip();

		Vector2 right = getRightVector ();	//The Vector in the left - Direction
		if (movingRight) {			
			dir = right * maxSpeed;		//move Right
		}else{			
			dir = -right * maxSpeed;	//move Left
		}

		transform.position += (Vector3) dir;	//Add dir to current Position

		fixPosition ();			//Fix the Position
	}
	
	/*Currently not used*/
	public void setCurSpaceObject (GameObject newSpaceObject){
		curSpaceObject = newSpaceObject;
		GetComponent<Enemy_Rotation> ().setCurrentSpaceObject (newSpaceObject);
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
