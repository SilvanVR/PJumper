using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetSystem : MonoBehaviour {

	public List<Transform> spaceObjectsTransform;
	public float[] speeds;
	public bool[] direction;

	private List<PlanetSystem_SpaceObject> spaceObjects;

	void Start () {
		spaceObjects = new List<PlanetSystem_SpaceObject> ();

		if (speeds.Length < spaceObjectsTransform.Count || direction.Length < spaceObjectsTransform.Count){
			Debug.Log ("PLANETENSYSTEM-ERROR: GRÖßE DES ARRAYS ANPASSEN (SPEED + RICHTUNG)"); 
			return;
		}

		for (int i = 0; i < spaceObjectsTransform.Count; i++) {
			Transform t = spaceObjectsTransform[i];
			Vector2 diffVec = t.position - transform.position;
			
			float ang = Vector2.Angle (Vector2.right,diffVec) * Mathf.Deg2Rad;
			if (transform.position.y > t.position.y) ang = -ang;
			
			spaceObjects.Add(
				new PlanetSystem_SpaceObject(t,diffVec.magnitude,direction[i],ang,speeds[i] * 0.01f));
		}
	}

	void Update () {
		foreach (PlanetSystem_SpaceObject so in spaceObjects) {
			so.calcNewPosition ((Vector2)transform.position);
		}
	}


}
