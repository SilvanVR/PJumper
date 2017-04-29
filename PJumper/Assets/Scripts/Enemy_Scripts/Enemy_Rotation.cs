using UnityEngine;
using System.Collections;

/*
 * This Script controls the Rotation from the Enemy, based on the GameObject "curSpaceObject"
 * curSpaceObject will be set in the Script "Enemy_Movement" in the Start-Function
 */
public class Enemy_Rotation : MonoBehaviour { 

	public float rotation_step = 10;

	private GameObject curSpaceObject;
	private float angle;
	
	void Start (){
		angle = 0;
	}
	
	void FixedUpdate(){
		if (curSpaceObject == null) return;
		Vector3 vectorToTarget = curSpaceObject.transform.position - transform.position; //Vector to current SpaceObject
		float tarAngle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg); //The desired Angle
		
		tarAngle += 180; //Dient zum Anpassen, somit kann tarAngle 0 - 360 sein
						
		float diff = Mathf.Abs(tarAngle - angle);		//Difference from tarAngle and Angle
		
		if(diff < 2*rotation_step || (360 - diff) < 2*rotation_step) angle = tarAngle;	//Fix the angle to tarAngle if the angle is in "range"
		else{
			float diffRight; //Calculates the right-side distance from Angle to TarAngle
			if(tarAngle > angle){
				diffRight = tarAngle - angle;
			}else{
				diffRight = 360 - angle + tarAngle;
			}						
			if(diffRight < 180) angle += rotation_step;
			else angle -= rotation_step;
		}
		transform.eulerAngles = new Vector3(0,0,angle - 90); //Attach the calculated Angle to the rigidbody's rotation
	}

	/*Set the current SpaceObject on which the rotation depends*/
	public void setCurrentSpaceObject (GameObject g){
		curSpaceObject = g;
	}
}
