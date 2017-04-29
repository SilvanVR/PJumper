using UnityEngine;
using System.Collections;

public class Player_Movement_v1 : MonoBehaviour {

	public float maxSpeed;				//The Max-Speed
	public float stopSpeed = 0.1f;		//The Stop-Speed	
	public float jumpPower = 100;		//The Jump-Power
	public float acceleration = 0.05f;	//The Acceleration
	public KeyCode jumpButton;

	private GameObject curSpaceObject;	//The Nearest Space-Object (this is where the Rotation is Calculated from)
	private float radius;				//Radius of the Player
	private bool jumping, doubleJump = false;	
	private float jumpPowerMin, jumpPowerMax;
	private GameObject curSpaceObjectJump;
	private bool grounded = false;		//True: Player is on a SpaceObject
	private bool inSpace = false;		//True: Player is not in a Gravity-Radius from a SpaceObject
	private bool facingRight = true;
	private Animator anim;
	private float curSpeed;				//The Current Speed
	private Vector2 dirVectorAsteroid;

	// Use this for initialization
	void Start () {
		radius = GetComponent<CircleCollider2D> ().radius;
		anim = GetComponent<Animator> ();
		jumpPowerMin = jumpPower;
		jumpPowerMax = 1.2f * jumpPower;
		setCurrentSpaceObject(GameObject.FindGameObjectWithTag ("Planet"));
	}

	void Update ()
	{
		if (!Input.anyKey)	anim.SetBool ("NoKeyPressed", true);
		else anim.SetBool ("NoKeyPressed", false);
		if (!grounded)	return;
		if (Input.GetKeyDown (jumpButton)) jumping = true;
		if (Input.GetKey (KeyCode.W)) jumpPower = jumpPowerMax;
		else jumpPower = jumpPowerMin;
	}

	void FixedUpdate () {
		
		float move = Input.GetAxis ("Horizontal"); 		//Read the Input
		//Flip the Image of the Player
		if (move > 0 && !facingRight) flip ();
		else if (move < 0 && facingRight) flip();

		/*Allows a Second Jump (with Half Power) only if the SpaceObject on which he jumped off is the current Space Object and he isnt in Space*/
		if (doubleJump && Input.GetKeyDown(jumpButton) && !inSpace && curSpaceObjectJump == curSpaceObject) {
			doubleJump = false;	
			Vector2 up = transform.position - curSpaceObject.transform.position;	//The Vector to the Up-Direction
			GetComponent<Rigidbody2D>().velocity += up * jumpPower/2;			//Add the Vector to current Velocitys
		}

		if (!grounded || curSpaceObject == null) return; //No Physic-Calculation if Player isnt on a Planet (or on other Object)

		if(Input.anyKey) curSpeed += Input.GetAxis ("Horizontal") * acceleration;	//Sets the current Speed based on the Input-Axis
		if (curSpeed > maxSpeed) curSpeed = maxSpeed;				//Set curSpeed to MaxSpeed if curSpeed is Higher		
		else if (curSpeed < -maxSpeed) curSpeed = -maxSpeed;		//Set curSpeed to MaxSpeed if curSpeed is Higher (other direction)

		Vector2 left = getLeftVector (transform.position, curSpaceObject.transform.position);	//The Vector in the left - Direction
		GetComponent<Rigidbody2D>().velocity = left * curSpeed;	//Set the current Velocity-Vector to the new Vector

		anim.SetFloat ("Speed", Mathf.Abs(curSpeed));

		if (!Input.anyKey && curSpeed != 0){	//Stop the Player constantly if he doesnt press a button
			curSpeed *= stopSpeed;
			if(Mathf.Abs(curSpeed) < 0.1f) curSpeed = 0;
		}

		if (jumping) {
			jumping = false;
			doubleJump = true;
			curSpaceObjectJump = curSpaceObject;
			grounded = false;
			Vector2 up = transform.position - curSpaceObject.transform.position;	//The Vector to the Up-Direction
			GetComponent<Rigidbody2D>().velocity += up * jumpPower;			//Add the Vector to current Velocity
			anim.SetBool("Jump",true);
		}

		fixPosition ();		//Fix the Position on a SpaceObject
	}

	/*Fix the Position from the Character is he is on a Planet*/
	void fixPosition (){
		Vector3 diff = (transform.position - curSpaceObject.transform.position).normalized;
		transform.position = curSpaceObject.transform.position + diff * (curSpaceObject.GetComponent<CircleCollider2D>().radius + radius + 0.01f);
	}

	/*Flip the Image from the Player*/
	void flip (){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	/*The Distance from the CurrentSpaceObject to the Player */
	public float getDistanceToCurSpaceObject(){
		if (curSpaceObject == null) return 0;
		return Vector3.Distance (curSpaceObject.transform.position, transform.position) - curSpaceObject.GetComponent<CircleCollider2D>().radius;
	}

	/*Sets the given GameObject to the new currentSpaceObject from the Player*/
	public void setCurrentSpaceObject (GameObject g){
		curSpaceObject = g;
		GetComponent<Player_Rotation> ().setCurrentSpaceObject (g);
	}

	public void setVelocityToZero (){
		GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);
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
		anim.SetBool ("Jump", false);
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

	public void addVectorToVelocity (Vector2 vec){
		GetComponent<Rigidbody2D>().velocity += vec;
	}

	public void setInSpace (bool b){ inSpace = b;}

	public bool getGrounded (){	return grounded;}

	public float getRadius (){ return radius;}

}
