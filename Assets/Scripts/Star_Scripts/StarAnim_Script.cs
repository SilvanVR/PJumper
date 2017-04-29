using UnityEngine;
using System.Collections;

public class StarAnim_Script : MonoBehaviour {

	public float anim_speed = 1;
	public float scale_speed = 0.05f;
	public float rotation_speed = -5f;

	
	private Vector3 scale;
	private Vector3 rotation;
	private bool state = true;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector2 (0,0);
		scale = new Vector3 (scale_speed, scale_speed, scale_speed);
		rotation = new Vector3 (0, 0, rotation_speed);
	}
	
	// Update is called once per frame
	void Update () {
		if (state){
			transform.localScale += scale;
			transform.Rotate (rotation);
			if (transform.localScale.x > 1) state = false;
		}else{
			transform.localScale -= scale;
			transform.Rotate (rotation);
			if (transform.localScale.x < 0) Destroy (gameObject);
		}
	}

}
