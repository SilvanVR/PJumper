using UnityEngine;
using System.Collections;

/*
 * This Script holds the data's for the Radius and the Gravity-Radius
 * Recalculated every frame in Update-Method.
 * This Script is mainly used by the Player_Movement Script for Fixing the Position on a Space Object.
 */
public class SpaceObjectDatas : MonoBehaviour {

	public float individualGravityStrength = 1;	//Allows to Modify the Individual-Gravity-Strength

	public float radius;					//Radius of the SpaceObject
	public float gravity_radius;			//Gravity-Radius of the Space-Object
	public float gravity_strength;			//Gravity-Strength

	private CircleCollider2D col;			//The Collider from the Space-Object

	void Awake (){
		col = GetComponent<CircleCollider2D> ();

		radius = col.radius * transform.localScale.x;
		gravity_radius = radius * GameManager.gravity_radius_multiply;
		gravity_strength = GameManager.main_Gravity * Mathf.Pow (radius,GameManager.gravity_size_strength)
						   * individualGravityStrength;			//Strength based on radius
	}

	//Recalculate Automatically the Radius and Gravity-Radius every Frame
	void Update (){
		radius = col.radius * transform.localScale.x;
		gravity_radius = radius * GameManager.gravity_radius_multiply;
	}
	
	public float getRadius (){return radius;}
	public float getGravityRadius (){return gravity_radius;}
}
