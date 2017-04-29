using UnityEngine;
using System.Collections;

public class Heart_Pulsate : MonoBehaviour {


	public float maxScale = 1.5f;
	public float minScale = 1f;
	public float animSpeed = 1f;

	private bool grow = true;

	void Update () {
		if (grow) {
			Vector2 oldScale = transform.localScale;
			transform.localScale = new Vector2 (oldScale.x + animSpeed, oldScale.y + animSpeed);
			if (transform.localScale.x >= maxScale) grow = false;
		}else{
			Vector2 oldScale = transform.localScale;
			transform.localScale = new Vector2 (oldScale.x - animSpeed, oldScale.y - animSpeed);	
			if (transform.localScale.x <= minScale) grow = true;
		}
	}
}
