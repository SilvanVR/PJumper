using UnityEngine;
using System.Collections;

/*
 * Calculates the Rotation based on the current Space-Object.
 */
public class Player_Rotation : MonoBehaviour {

	public float rotation_step = 5;
	public float range_strength = 2;

	private GameObject curSpaceObject;
	private float angle = 0;
 
	void FixedUpdate(){
		if (curSpaceObject == null) return;
		Vector3 vectorToTarget = curSpaceObject.transform.position - transform.position; //Vector to current SpaceObject
		float tarAngle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg); //The desired Angle

		float curSoRadius = curSpaceObject.GetComponent<SpaceObjectDatas>().radius;
		//The distance to the Target (from ground to feet)
		float distanceToTarget = vectorToTarget.magnitude - curSoRadius - GetComponent<Player_Movement> ().getRadius (); 

		tarAngle += 180; //Dient zum Anpassen, somit kann tarAngle 0 - 360 sein

		//Debug.Log ("Angle: " + angle + " TarAngle: " + tarAngle);

		//Rotation-Speed depends on the Distance from Player to curSpaceObject
		float rotation_speed = 2 * Mathf.Pow ((1 - (distanceToTarget / (curSpaceObject.GetComponent<SpaceObjectDatas> ().gravity_radius - curSoRadius))),range_strength); //bestimmt Rotationsgeschwindigkeit je nach Entfernung zum Planet

		float diff = Mathf.Abs(tarAngle - angle);		//Difference from tarAngle and Angle
		
		if(diff < 20 || (360 - diff) < 20) angle = tarAngle;	//Fix the angle to tarAngle is the angle is in "range"
		else{
			float diffRight; //Calculates the right-side distance from Angle to TarAngle
			if(tarAngle > angle){
				diffRight = tarAngle - angle;
			}else{
				diffRight = 360 - angle + tarAngle;
			}						
			if(diffRight < 180) angle += rotation_step * rotation_speed;
			else angle -= rotation_step * rotation_speed;
		}
		GetComponent<Rigidbody2D>().rotation = angle - 90; //Attach the calculated Angle to the rigidbody's rotation
	}

	/* Set the current SpaceObject on which the rotation depends
	  Called from: Player_Movement */
	public void setCurrentSpaceObject (GameObject g){
		curSpaceObject = g;
	}
}
