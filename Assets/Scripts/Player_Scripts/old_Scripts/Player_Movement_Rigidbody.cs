using UnityEngine;
using System.Collections;

public class Player_Movement_Rigidbody : MonoBehaviour {

	public float maxSpeed;				//The Max-Speed
	public float stopSpeed = 0.1f;		//The Stop-Speed	
	public float jumpPower;		//The Jump-Power
	public float acceleration = 0.05f;	//The Acceleration
	public KeyCode jumpButton;
	public KeyCode moveLeftButton;
	public KeyCode moveRightButton;
	
	private GameObject curSpaceObject;	//The Nearest Space-Object (this is where the Rotation is Calculated from)
	private float height, startHeight;				//Height and Start-Height (to reset it)
	private bool jumping, doubleJump = false;	
	private float jumpPowerMin, jumpPowerMax;
	private GameObject curSpaceObjectJump;		//saves the currentSpaceObject when Player jumps, so he can only do a double-jump on the same SO
	private bool grounded = false;		//True: Player is on a SpaceObject
	private bool inSpace = false;		//True: Player is not in a Gravity-Radius from a SpaceObject
	private bool facingRight = true;
	private Animator anim;
	private float curSpeed;				//The Current Speed
	private Vector2 dirVectorAsteroid;
	
	// Use this for initialization
	void Start () {
		height = startHeight = GetComponent<BoxCollider2D> ().size.y;
		anim = GetComponent<Animator> ();
		jumpPowerMin = jumpPower;
		jumpPowerMax = 1.4f * jumpPower;
		setCurrentSpaceObject(GameObject.FindGameObjectWithTag ("Planet"));
	}
	
	void Update ()
	{
		Debug.Log (GetComponent<Rigidbody2D>().velocity.magnitude);
		if (!moveLeft() && !moveRight()) anim.SetBool ("NoKeyPressed", true);
		else anim.SetBool ("NoKeyPressed", false);
		
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))
			GetComponent<Rigidbody2D>().velocity = new Vector2 (20 * Input.GetAxis ("Horizontal"),20 * Input.GetAxis ("Vertical"));
		
		if (!grounded)	return;
		if (Input.GetKey (jumpButton)) jumping = true;
		if (Input.GetKey (KeyCode.W)) jumpPower = jumpPowerMax;
		else jumpPower = jumpPowerMin;
		
	}
	
	void FixedUpdate () {
		//Flip the Image of the Player
		if (moveLeft () && facingRight) flip ();
		else if (moveRight () && !facingRight) flip();
		
		/*Allows a Second Jump only if the SpaceObject on which he jumped off is the current Space Object and he isnt in Space*/
		if (doubleJump && Input.GetKeyDown(jumpButton) && !inSpace && curSpaceObjectJump == curSpaceObject) {
			doubleJump = false;	
			Vector2 up = (transform.position - curSpaceObject.transform.position).normalized;	//The Vector to the Up-Direction
			GetComponent<Rigidbody2D>().velocity += up * jumpPower;			//Add the Vector to current Velocity
		}
		
		if (!grounded || curSpaceObject == null) return; //No Physic-Calculation if Player isnt on a Planet (or on other Object)
		
		if (moveLeft ()) curSpeed -= acceleration;
		else if (moveRight ()) curSpeed += acceleration;
		
		if (curSpeed > maxSpeed) curSpeed = maxSpeed;				//Set curSpeed to MaxSpeed if curSpeed is Higher		
		else if (curSpeed < -maxSpeed) curSpeed = -maxSpeed;		//Set curSpeed to MaxSpeed if curSpeed is Higher (other direction)
		
		Vector2 left = getLeftVector (transform.position, curSpaceObject.transform.position);	//The Vector in the left - Direction
		GetComponent<Rigidbody2D>().velocity = left * curSpeed;	//Set the current Velocity-Vector to the new Vector
		
		anim.SetFloat ("Speed", Mathf.Abs(curSpeed));
		
		if (!moveLeft() && !moveRight() && curSpeed != 0){	//Stop the Player constantly if he doesnt press a button
			curSpeed *= stopSpeed;
			if(Mathf.Abs(curSpeed) < 0.1f) curSpeed = 0;
		}
				
		if (jumping) {
			jumping = false;
			doubleJump = true;
			curSpaceObjectJump = curSpaceObject;
			grounded = false;
			Vector2 up = (transform.position - curSpaceObject.transform.position).normalized;	//The Vector to the Up-Direction
			GetComponent<Rigidbody2D>().velocity += up * jumpPower;			//Add the Vector to current Velocity
			anim.SetTrigger("Jump");
			anim.SetBool ("Grounded",false);
		}
		fixPosition ();		//Fix the Position on a SpaceObject
	}
	
	/*Fix the Position from the Character is he is on a Planet*/
	private void fixPosition (){
		Vector3 diff = (transform.position - curSpaceObject.transform.position).normalized;
		transform.position = curSpaceObject.transform.position + diff * (getCurSpaceObjectRadius () + getRadius () + 0.01f); //+0.01f: Set the Player a little higher than the collider, so he doesnt trigger always OnColliderEnter 
	}
	
	/*Returns the Radius from the current bounded Space-Object */
	private float getCurSpaceObjectRadius (){
		return curSpaceObject.GetComponent<CircleCollider2D> ().radius * curSpaceObject.transform.localScale.x;
	}
	
	/*Flip the Image from the Player*/
	private void flip (){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	/*Read the Input-buttons (chosen in Inspector) and return its bool-Value*/
	private bool moveLeft (){return Input.GetKey (moveLeftButton);}
	private bool moveRight (){return Input.GetKey (moveRightButton);}
	
	/*The Distance from the CurrentSpaceObject to the Player */
	public float getDistanceToCurSpaceObject(){
		if (curSpaceObject == null) return 0;
		return Vector3.Distance (curSpaceObject.transform.position, transform.position) - curSpaceObject.GetComponent<SpaceObjectDatas>().radius;
	}
	
	/*Sets the given GameObject to the new currentSpaceObject from the Player*/
	public void setCurrentSpaceObject (GameObject g){
		curSpaceObject = g;
		GetComponent<Player_Rotation> ().setCurrentSpaceObject (g);
	}
	
	/*Set the Velocity to Zero (No Movement then)*/
	public void setVelocityToZero (){
		GetComponent<Rigidbody2D>().velocity.Scale (new Vector2 (0, 0));
		GetComponent<Rigidbody2D>().angularVelocity = 0; 
	}
	
	/*Called from the CircleCollider from the SpaceObject if it Collides with the Player*/
	public void setGroundedTrue (){
		if(grounded) return;
		Vector2 normalComponent = getNormalComponent (transform.position,curSpaceObject.transform.position);
		Vector2 left = getLeftVector (transform.position, curSpaceObject.transform.position);
		
		float dir = Vector2.Dot (left,GetComponent<Rigidbody2D>().velocity); //negative : left , positive: right
		
		if (dir < 0) {
			if (facingRight) flip ();
			curSpeed = -normalComponent.magnitude;
		}
		else if (dir > 0){
			if (!facingRight) flip ();
			curSpeed = normalComponent.magnitude;
		}
		else curSpeed = 0;
		
		grounded = true;
		anim.SetBool ("Grounded",true);
	}
	/*Return a Vector which is a normal-Vector (90 Degree) to the diff-Vector from the 2 given Positions*/
	public Vector2 getLeftVector (Vector2 pos1, Vector2 pos2){
		float diffX = pos1.x - pos2.x;
		float diffY = pos1.y - pos2.y;
		float diffBetrag = (float)Mathf.Sqrt(diffX * diffX + diffY * diffY);
		return new Vector2 (diffY / diffBetrag, -diffX / diffBetrag);
	}
	
	/*Returns the Normal-Component of the Diff-Vector between those 2 given Positions and the Direction-Vector from the Player (Velocity-Vector)*/	
	//Normalkomponente: b - (a*b / a*a) * a
	public Vector2 getNormalComponent (Vector2 pos1, Vector2 pos2){
		Vector2 diffVector = pos1 - pos2;
		Vector2 direction = GetComponent<Rigidbody2D>().velocity;
		float a = direction.x * diffVector.x + direction.y * diffVector.y; //a * b
		float b = diffVector.x * diffVector.x + diffVector.y * diffVector.y; //a * a
		float c = a / b; // a*b/a*a
		return direction - c * diffVector; //b - (a*b / a*a) * a
	}
	
	/*Scale the Player (called from BlackHoles)*/
	public void scalePlayer (float scale){
		if (scale < 0) scale = 0;
		transform.localScale = new Vector2 (scale, scale);
	}	
	
	public bool getGrounded (){	return grounded;}
	public GameObject getCurrentSpaceObject (){ return curSpaceObject;}	
	public float getRadius (){ return height/2;}	
	public float getStartHeight () { return startHeight;}
	public float getDistanceToCurPlanet (){	return Vector3.Distance (transform.position, curSpaceObject.transform.position);}	
	public void setInSpace (bool b){ inSpace = b;}
	
	public void burnToDeath(){
		GetComponent<Rigidbody2D>().Sleep (); 	//Stop Rigidbody of moving
		anim.SetTrigger ("Burning");	//Start burning-Animation ---> Calls kill () at the End
	}
	
	public void kill (){
		Application.LoadLevel (Application.loadedLevel);
	}

}
