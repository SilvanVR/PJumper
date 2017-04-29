using UnityEngine;
using System.Collections;

/*
 * This Script calculates the Gravity-Vector from a Blackhole
 * (based on how far the Player is from the SpaceObject away and how strong the gravity is)
 */
public class Blackhole_Gravity : Gravity_Radius {

	public float killRadius = 0.3f; 	//The Radius in which the Player will be killed
	public float drawnIntoBlackholeRadius = 0.6f; //The Radius (Scale) in which the Player will be drawn into the Blackhole (if he is on a Planet)
	public bool gravityOnAsteroids = false;

	private bool calledOnce = false;
		
	//Calculates the whole Gravity.
	void OnTriggerStay2D (Collider2D other){
		//Debug.Log ("Sth in my Trigger.. " + other);
		#if UNITY_EDITOR
		Debug.DrawLine (transform.position, other.transform.position, Color.green);
		#endif
	
		if (other.tag == "Player")  {		
			Player_Movement player_mov = other.GetComponent<Player_Movement> ();
			other.GetComponent<Player_TimeInSpace> ().setInSpace (false);

			Vector2 diff_Vector = transform.position - other.transform.position;		//The Difference Vector from the SpaceObject to the Player
			Vector2 diff_Vector_norm = diff_Vector.normalized;					//The normalized Difference Vector
			float distance = diff_Vector.magnitude;	//The Distance to Blackhole-Center

			float range_power = Mathf.Pow((1 - (distance / datas.gravity_radius)),gravity_range_strength); //Calculate the Strength of the Gravity based on how far the Player is from it
			if (range_power < 0) range_power = 0;
		
			if (distance < datas.radius){ //Player in Center of Blackhole?	
				float scale = diff_Vector.magnitude / datas.radius; //Calculate the Scale (0-1)
				player_mov.scalePlayer (scale);	//Scale the Player based on the distance to Center
				//Add specialised Grav-Vector to Player (Prevents Player from escaping out of hole)
				player_mov.addGravHole(diff_Vector_norm * datas.gravity_strength * range_power,0.98f);
				if (distance < killRadius)	other.GetComponent<Player_Health>().kill (); //Player get's killed
			}else{ //Add an other Gravity-Vector to the Player (this one is more weak)
				player_mov.addGravHole(diff_Vector_norm * datas.gravity_strength * range_power,0.998f);  
			}

			//If this SpaceObject is nearer than the currentSpaceObject from the Player, this will be the new currentSpaceObject
			if (diff_Vector.magnitude - datas.radius < player_mov.getDistanceToCurSpaceObject())
			{ 
				player_mov.setCurrentSpaceObject (parentObject);
			}

			if (distance < (datas.gravity_radius * drawnIntoBlackholeRadius) && !calledOnce) //Player will be drawn into the Blackhole if he is to close 
			{
				player_mov.releasePlayerFromCurSpaceObject ();
				calledOnce = true;
			}

		}else if (other.tag == "Asteroid"){ 
			//Gravity will be only calculated in the TriggerCollider from the Blackhole, otherwise the Gravity wont be calculated to the Player,
			//because the Player is a Child from the Asteroid
			if (!gravityOnAsteroids) return;
					
			Vector2 diff_Vector = transform.position - other.transform.position;		//The Difference Vector from the SpaceObject to the Player
			Vector2 diff_Vector_norm = diff_Vector.normalized;					//The normalized Difference Vector
			float distance = diff_Vector.magnitude;	//The Distance to Blackhole-Center
			
			if (distance < datas.radius){ 	
				float range_power = Mathf.Pow((1 - (distance / datas.gravity_radius)),gravity_range_strength); 
				if (range_power < 0) range_power = 0;
				
				other.transform.position += (Vector3)diff_Vector_norm * datas.gravity_strength * range_power;

				float scale = diff_Vector.magnitude / datas.radius; //Calculate the Scale (0-1)
				other.transform.localScale = new Vector3(scale,scale,scale);	

				if (distance < killRadius)	other.GetComponent<Asteroid_Movement>().respawn (); 
			}

			/*
			if (distance < (datas.gravity_radius * drawnIntoBlackholeRadius)) //Player will be drawn into the Blackhole if he is to close 
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().releasePlayerFromCurSpaceObject ();
			}
			*/


		}
	}
}
