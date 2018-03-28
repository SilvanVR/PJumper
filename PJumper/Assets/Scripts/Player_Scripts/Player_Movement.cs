using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * This Script controls the Movement of the Player (and a little bit more)
 */

public class Player_Movement : MonoBehaviour {

	public Transform playerParent;	//Player is Child of this if he IS NOT on a SpaceObject
	public float maxSpeed;		//The Max-Speed
	public float stopSpeed;		//The Stop-Speed	
	public float jumpPower;		//The Jump-Power
	public float acceleration;	//The Acceleration
	public bool affectedByGravityOnASpaceObject = true; 
	public bool affectedByGravity = true;
	public KeyCode jumpButton;
	public KeyCode moveLeftButton;
	public KeyCode moveRightButton;
	public bool grounded = false;		//True: Player is on a SpaceObject
	public GameObject smokeEffect;
	public UI_Button uI_LeftButton;
	public UI_Button uI_RightButton;
	public UI_Button uI_jumpButton;
		
	private float curSpeed;				//The Current Speed
	private GameObject curSpaceObject;	//The Nearest Space-Object (this is where the Rotation is Calculated from)
	private GameObject curSpaceObject_Jump; //The Space-Object on which the Player jumped off
	private float jump_delay; //Stores the Start-Time of an Jump (for a double Jump - only allowed after 0.5s after a normal-jump)
	private float height, startHeight;				//Height and Start-Height (to reset it)
	private bool jumping, doubleJump = false;		//jumping-true: Jumping is possible (doubleJump same)
	private bool doubleJumpTrait = false;	//Disable double-Jump till it's bought from the Shop
	private bool facingRight = true;	//True: Player is facing right
	private bool stopMoving = false;		//True: Player cant move anymore		
	private bool jumpPossible = true;	 //Disable ability to jump (If the Level is Complete)
	private Animator anim;
	private Vector2 dir;
	private float blockJumpIfGrounded;

	void Start () {
		height = startHeight = GetComponent<BoxCollider2D> ().size.y;
		anim = GetComponent<Animator> ();
		GameManager.instance.addPlayer (this.gameObject); //Notify the GameManager to add a new Player
	}
	
	void Update ()
	{
		#if UNITY_EDITOR 	
			if (Input.GetKey (KeyCode.R)) Application.LoadLevel(Application.loadedLevel); //Restart Level
			if (Input.GetKeyDown (KeyCode.T)) GetComponent<Player_GunControl> ().triggerGun (); 
		#endif

		if (!movingLeft() && !movingRight()) anim.SetBool ("NoKeyPressed", true);
		else anim.SetBool ("NoKeyPressed", false);

		/*
		//CHEATING (REMOVE LATER)
		if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))
		{
			transform.position += new Vector3 (4*Input.GetAxis ("Horizontal"),4*Input.GetAxis ("Vertical"),0); 
			dir = Vector2.zero;
		}*/

		if (!grounded)	return;
		if ((Time.time - blockJumpIfGrounded > 0.3f) && jumpPressed ()) jumping = true;			//Allows Jumping if Player press the "jumpButton"
		//if (Input.GetKey (KeyCode.W)) jumpPower = jumpPowerMax; //Allows higher Jump if "W" is pressed
		//else jumpPower = jumpPowerMin;
	}

	/*
	 * Calculates the Movement:
	 * Every Fixed Update (0.02s) the direction Vector (dir) will be add to the Transform-Position.
	 * The direction-Vector is modified by all Space-Objects (to simulate Physics)
	 * and in this Script if the Player is actually moving.
	 */
	void FixedUpdate () {
		if (stopMoving) return; //Prevents controlling the Player

		bool moving = movingLeft () || movingRight (); //Player is Moving

		if (grounded){ //Player is on a SpaceObject (Planet etc.)
			if (moving){ //Player is Moving
				if (movingLeft ()) moveLeft ();
				else if (movingRight ()) moveRight ();

				Vector2 right = getRightVector ();	//The Vector in the Right - Direction
				dir = right * curSpeed;	//Set the current Direction-Vector to the new Vector
						
			}else{ //Player is on a SpaceObject but is not moving (no button pressed)
				dir *= stopSpeed; 			//Stop the Player constantly 
				curSpeed *= stopSpeed;		//Stop the Player constantly 
				if(Mathf.Abs(curSpeed) < 0.01f) curSpeed = 0;
			}
			
			if (jumping && jumpPossible) jump ();

			anim.SetFloat ("Speed", Mathf.Abs(dir.magnitude));
		}else{	//Player is not on a Space-Object
			//Flip the Image of the Player
			if (movingLeft () && facingRight) flip ();
			else if (movingRight () && !facingRight) flip();

			/*Allows a Second Jump if the curSpaceObject didnt changed + Player is not in Space + after 0.5s after the first jump*/
			if (doubleJumpTrait && doubleJump && jumpPressed () 
			    && !GetComponent<Player_TimeInSpace> ().getInSpace() && curSpaceObject_Jump == curSpaceObject
			    && (Time.time - jump_delay > 0.5f)) {
				doubleJump = false;							//Disable Double Jump
				dir += getUpVector () * jumpPower;			//Add the Vector to current dir-Vector
				Instantiate (smokeEffect,transform.Find ("JumpDustSpawn").transform.position,Quaternion.identity);
			}
		}

		transform.position += (Vector3) dir; //Add final directon-Vector to Position	

		if (grounded) fixPosition (); //Fix the Position on the SpaceObject if Player is grounded	
		
		#if UNITY_EDITOR 	
		Debug.DrawLine (transform.position, transform.position + (Vector3)dir * 5, Color.red); //Some Debug Stuff (REMOVE LATER)	
		if (curSpaceObject != null) Debug.DrawLine (transform.position, curSpaceObject.transform.position, Color.yellow);
		#endif
	}

	//---------------------------------------------------- EVENT - METHODS ---------------------------------------------------------------------------
	/*Read the Input-buttons (chosen in Inspector) and return it's bool-Value*/
	/*private bool movingLeft (){return Input.GetKey (moveLeftButton);}
	private bool movingRight (){return Input.GetKey (moveRightButton);}*/

	private bool movingLeft (){return uI_LeftButton.isPressed || Input.GetKey (moveLeftButton) || Input.GetKey (KeyCode.LeftArrow);}
	private bool movingRight (){return uI_RightButton.isPressed || Input.GetKey (moveRightButton) || Input.GetKey (KeyCode.RightArrow);}
	private bool jumpPressed (){return uI_jumpButton.isPressed || Input.GetKey (jumpButton) || Input.GetKey (KeyCode.UpArrow);}
	
	public void moveLeft (){
		curSpeed -= acceleration;
		if (curSpeed < -maxSpeed) curSpeed = -maxSpeed;				//Set curSpeed to MaxSpeed if curSpeed is Higher	
		if (facingRight) flip ();
	}

	public void moveRight (){		
		curSpeed += acceleration;
		if (curSpeed > maxSpeed) curSpeed = maxSpeed;				//Set curSpeed to MaxSpeed if curSpeed is Higher 
		if (!facingRight) flip ();
	}

	//Called if Jump-Button is pressed and Player is grounded
	private void jump (){
		doubleJump = true;							//Enable double Jump
		jumping = grounded = false;					//Disable Normal-Jump and set Grounded to false
		curSpaceObject_Jump = curSpaceObject;		//Saves the SpaceObject on which the Player jumped off
		jump_delay = Time.time;						//Saves Start-Time of the jump
		dir += getUpVector () * jumpPower;			//Add the Vector to current dir-Vector
		transform.SetParent (playerParent);			//SET PLAYERPARENT AS THE NEW PARENT!!
		anim.SetTrigger("Jump");
		anim.SetBool ("Grounded",false);
		Instantiate (smokeEffect,transform.Find ("JumpDustSpawn").transform.position,Quaternion.identity);	//Some Dust-Effect
	}

	/* Called from the CircleCollider from the SpaceObject if it Collides with the Player
	 * Calculates the Sliding-Direction and Strength.
	 */
	public void setGroundedTrue (GameObject colliderSpaceObject){
		if(grounded) return;
		setCurrentSpaceObject (colliderSpaceObject);	//Set the Collided-Object as the new CurrentSpaceObject
		transform.SetParent (curSpaceObject.transform); //SET CURRENTSPACEOBJECT AS THE NEW PARENT!!!

		Vector2 normalComponent = getNormalComponent (transform.position,curSpaceObject.transform.position);
		Vector2 right = getRightVector (); //Normalized-Vector to the Right
		
		float direction = Vector2.Dot (right,dir); //negative : left , positive: right
		
		if (direction < 0) {
			if (facingRight) flip ();
			dir = -normalComponent.magnitude * right;
		}
		else if (direction > 0){
			if (!facingRight) flip ();
			dir = normalComponent.magnitude * right;
		}
		else setDirToZero ();

		if (colliderSpaceObject != curSpaceObject_Jump) blockJumpIfGrounded = Time.time;	//Blocks the Jump for a little if Player landed on another Planet
		curSpeed = 0;	//Resets Speed from Player is he land on an Space-Object (looks more better, but can be removed)
		grounded = true;
		anim.SetBool ("Grounded",true);
	}

	//----------------------------------------------------- PUBLIC METHODS ---------------------------------------------------------------------------

	/*Adds the given Vector to the dir-Vector to simulate Gravity (Called from all Space-Objects to simulate Physics)*/
	public void addGrav (Vector2 vec){		
		if (!affectedByGravityOnASpaceObject && grounded || !affectedByGravity) return; //No Gravity from other Space-Objects if Player is on a SpaceObject
		dir += vec;
	}

	/*Same as "addGrav ()" - Called from BlackHoles (Prevents Player from escape out of it)*/
	public void addGravHole (Vector2 vec, float strength){
		addGrav (vec);
		dir *= strength; //Necessary, so Player will come closer and closer to center until he die
	}
		
	/*The Distance from the CurrentSpaceObject to the Player - Called from Gravity-Scripts to check which Planet is the new CurrentSpaceObject*/
	public float getDistanceToCurSpaceObject(){
		if (curSpaceObject == null) return 8192;
		return Vector3.Distance (curSpaceObject.transform.position, transform.position) - curSpaceObject.GetComponent<SpaceObjectDatas>().radius;
	}

	/*Called from Blackhole_Gravity-Script (Drawn the Player into a Blackhole if he is to close) + Asteroid-Movement (If the Player is on it during Respawn)*/
	public void releasePlayerFromCurSpaceObject (){
		grounded = false;
		GetComponent<Player_TimeInSpace> ().setInSpace (true);
		transform.SetParent (playerParent);			//SET PLAYERPARENT AS THE NEW PARENT!!
		anim.SetBool ("Grounded",false);
		anim.SetTrigger("Jump");
	}
	
	/*Sets the given GameObject to the new currentSpaceObject from the Player*/
	public void setCurrentSpaceObject (GameObject g){
		curSpaceObject = g;
		GetComponent<Player_Rotation> ().setCurrentSpaceObject (g);
	}

		
	/*Returns a Normalized-Vector (magnitude 1) which is a normal-Vector (90 Degree) to the diff-Vector*/
	public Vector2 getRightVector (){
		float diffX = transform.position.x - curSpaceObject.transform.position.x;
		float diffY = transform.position.y - curSpaceObject.transform.position.y;
		float diffBetrag = (float)Mathf.Sqrt(diffX * diffX + diffY * diffY);
		return new Vector2 (diffY / diffBetrag, -diffX / diffBetrag);
	}

	/*Returns the Vector to the Up-Direction from the current Space-Object*/
	public Vector2 getUpVector (){
		return (transform.position - curSpaceObject.transform.position).normalized;
	}
	
	/*Returns the Normal-Component of the Diff-Vector between those 2 given Positions and the Direction-Vector from the Player (Velocity-Vector)*/	
	//Normalkomponente: b - (a*b / a*a) * a
	public Vector2 getNormalComponent (Vector2 pos1, Vector2 pos2){
		Vector2 diffVector = pos1 - pos2;
		float a = dir.x * diffVector.x + dir.y * diffVector.y; //a * b
		float b = diffVector.x * diffVector.x + diffVector.y * diffVector.y; //a * a
		float c = a / b; // a*b/a*a
		return dir - c * diffVector; //b - (a*b / a*a) * a
	}

	/*Scale the Player (called from BlackHoles)*/
	public void scalePlayer (float scale){
		if (scale < 0.05f) return;
		float locScaleX = transform.localScale.x;
		if (locScaleX > 0) transform.localScale = new Vector2 (scale, scale);
		else transform.localScale = new Vector2 (-scale, scale);
	}	

	//public void disableDoubleJump(){doubleJump = false;} //Called from Player_TimeInSpace - Script (OnTriggerExit2D)
	public bool getGrounded (){return grounded;} //Called from GravityRadius - Script
	public GameObject getCurrentSpaceObject (){return curSpaceObject;}	//Called from GravityRadius / Blackhole_Gravity
	public float getRadius (){return height/2;}	//Called from Blackhole_Gravity
	public float getStartHeight () {return startHeight;} //Called from Blackhole_Gravity	
	public void stopMove (){stopMoving = true; setDirToZero ();} //Called from Player_TimeInSpace + Player_Health
	protected bool getFacingRight (){return facingRight;} //Called from the Gun-Script
	public void setDirToZero (){dir = Vector2.zero;}
	public void disableJump (){jumpPossible = false;}
	public void enableDoubleJump (){doubleJumpTrait = true;}
	public void resetScript (){transform.SetParent (playerParent);stopMoving = false;affectedByGravity = true; setDirToZero (); if (!facingRight) flip (); grounded = false; curSpaceObject = null;}

	//---------------------------------------------------------------- PRIVATE METHODS ------------------------------------------------------------------------	

	/*Fix the Position from the Character is he is on a Planet*/
	private void fixPosition (){	
		Vector3 diff = (transform.position - curSpaceObject.transform.position).normalized;
		transform.position = curSpaceObject.transform.position + diff * (getCurSpaceObjectRadius () + getRadius () + 0.01f); //+0.01f: Set the Player a little higher than the collider, so he doesnt trigger always OnColliderEnter 
	}
	
	/*Returns the Radius from the current bounded Space-Object */
	private float getCurSpaceObjectRadius (){
		return curSpaceObject.GetComponent<SpaceObjectDatas> ().radius;
	}
	
	/*Flip the Image from the Player*/
	private void flip (){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
