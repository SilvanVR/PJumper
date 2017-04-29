using UnityEngine;
using System.Collections;

/*
 * This Script calculates the Gravity-Vector from PLANETS + ASTEROIDS + SUNS
 * (based on how far the Player is from the SpaceObject away and how strong the gravity is)
 */
public class SpaceObject_GravityRadius : Gravity_Radius {

	float timerToChangeCurSpaceObject = 0; //Allows change the curSpaceObject only every 0.5 second.

	//Calculates the Gravity.
	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "Player") {
			//DEBUG STUFF (REMOVE LATER)
			Debug.DrawLine (transform.position, other.transform.position, Color.green);

			Player_Movement player_mov = other.GetComponent<Player_Movement> ();	//Get Player_Movement Script
			other.GetComponent<Player_TimeInSpace> ().setInSpace (false);			//Set Player in Space to FALSE

			if (player_mov.getCurrentSpaceObject() == parentObject && player_mov.getGrounded ()) return; //No Gravity from the current SpaceObject (If Player is grounded)

			Vector2 diff_Vector = transform.position - other.transform.position;		//The Difference Vector from the SpaceObject to the Player

			float distance = diff_Vector.magnitude - datas.radius - player_mov.getRadius();	//The Distance (Float-Val) from Ground to Player-Feet
			if (distance < 0) distance = 0;												//Just to be sure, that distance wont be negative

			float range_power = Mathf.Pow((1 - (distance / (datas.gravity_radius - datas.radius))),gravity_range_strength); //Calculate the Strength of the Gravity based on how far the Player is from the Planet
			if (range_power < 0) range_power = 0;

			Vector2 gravity_Vector = diff_Vector.normalized * datas.gravity_strength * range_power;
			player_mov.addGrav(gravity_Vector); //Add the final Gravity-Vector to the Player

			//If this SpaceObject is nearer than the currentSpaceObject from the Player, this will be the new currentSpaceObject
			if (diff_Vector.magnitude - datas.radius < player_mov.getDistanceToCurSpaceObject()){				
				if (Time.time - timerToChangeCurSpaceObject < 0.5f) return; //Allows change the curSpaceObject only every 0.5 second.
				timerToChangeCurSpaceObject = Time.time;
				player_mov.setCurrentSpaceObject (parentObject);
			}
		}
	}
}
