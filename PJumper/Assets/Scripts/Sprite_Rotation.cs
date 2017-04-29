using UnityEngine;
using System.Collections;

public class Sprite_Rotation : MonoBehaviour {

	public float rotation_speed_z;
	public float rotation_speed_y;

	private Transform t;

	// Use this for initialization
	void Start () {
		t = this.transform;
		rotation_speed_z = rotation_speed_z / 10;
		rotation_speed_y = rotation_speed_y / 10;
	}
	
	// Update is called once per frame
	void Update () {
		t.Rotate (0, rotation_speed_y, rotation_speed_z);
	}
}
