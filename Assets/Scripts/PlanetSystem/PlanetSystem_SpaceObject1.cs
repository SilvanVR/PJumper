using UnityEngine;
using System.Collections;

/*
 * This Script moves the Space-Object around another one (in a Circle).
 * Take's the Position in the Inspector IF LANE = 0.
 */
public class PlanetSystem_SpaceObject1 : MonoBehaviour {

	public Transform origin;	//The Origin where the SpaceObject is moving around
	public float speed = 1;		//The Moving-Speed
	public bool left;			//Determines the Direction
	public int lane = 0;
	public int laneRange = 50;
		
	private float angle;		//The Current-Angle
	private float distance;		//The Distance to the Origin
	
	void Start (){
		Vector2 diffVec = origin.position - transform.position;	//Difference-Vector between Origin and own Position	
		
		angle = Vector2.Angle (-Vector2.right,diffVec) * Mathf.Deg2Rad;	//Calculate the current Angle 
		if (transform.position.y < origin.position.y) angle = -angle;   //Correct the Angle

		speed *= 0.01f;	//Modify the Speed, so the Values in the Inspector are higher
	}

	void Update (){
		calcNewPosition (); //Calculate new Position every Frame
	}
		
	private void calcNewPosition (){
		if (lane == 0) distance = (origin.position - transform.position).magnitude;		//Distance from Origin to own Position
		else distance = lane * laneRange;												//Set Space-Object on a Lane

		if (left) {
			angle += speed * Time.deltaTime;	
		}else{
			angle -= speed * Time.deltaTime;
		}
		angle = angle % (2*Mathf.PI);
		transform.position = new Vector2 (origin.position.x + distance * Mathf.Cos (angle),
		                              	  origin.position.y + distance * Mathf.Sin (angle));
	}
}
