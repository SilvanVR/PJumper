using UnityEngine;
using System.Collections;

/*
 * The Super-Class from all Gravity-Radius Script's (who calculate the Gravity).
 */
public abstract class Gravity_Radius : MonoBehaviour {

	public float gravity_range_strength = 1;	//Determines the Strength of the Gravity, based on the Distance to the Planet
	
	protected GameObject parentObject; 		//The Parent-Object (Space-Object)
	protected SpaceObjectDatas datas;		//Hold's all Values for the SpaceObjects (like radius,gravity-radius)

	// OPTIMIZE: CALCULATE WITHOUT THE SCALE PARAMETER (BECAUSE IT SUCKS)
	void Start () {
		parentObject = transform.parent.gameObject; //Parent GameObject
		datas = parentObject.GetComponent<SpaceObjectDatas> (); //The Data Script

		float scale = parentObject.transform.localScale.x;	//The Local-Scale of the Parent-Object
		GetComponent<CircleCollider2D> ().radius = datas.gravity_radius / scale;  //Sets the Radius of the CircleCollider (The Trigger-Collider) to the gravity-radius
	}
		
	/*
	 * Set Player "InSpace" to TRUE if he left the Trigger-Radius (Automatically reset to False if he is an another Gravity-Radius)
	 */
	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "Player") {
			other.GetComponent<Player_TimeInSpace> ().setInSpace (true);		
		}
	}


}
