using UnityEngine;
using System.Collections;

public class PlanetSystem_SpaceObject
{
	
	public Transform trans;
	public float distance;
	public float speed;
	public float angle;
	public bool left;

	public PlanetSystem_SpaceObject (Transform t, float distance, bool dir){
		trans = t;
		this.distance = distance;
		left = dir;
		angle = 0;
		speed = 0.001f;
	}

	public PlanetSystem_SpaceObject (Transform t, float distance, bool dir, float angle, float speed){
		trans = t;
		this.distance = distance;
		left = dir;
		this.angle = angle;
		this.speed = speed;
	}

	public void calcAngle (){
		if (left) {
			angle += speed * Time.deltaTime % Mathf.PI;	
		}else{
			angle -= speed * Time.deltaTime % Mathf.PI;
		}
	}

	public void calcNewPosition (Vector2 origin){
		if (left) {
			angle += speed * Time.deltaTime % Mathf.PI;	
		}else{
			angle -= speed * Time.deltaTime % Mathf.PI;
		}
		trans.position = new Vector2 (origin.x + distance * Mathf.Cos (angle),
		                                 origin.y + distance * Mathf.Sin (angle));
	}

}

