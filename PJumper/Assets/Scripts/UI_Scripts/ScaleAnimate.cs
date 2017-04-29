using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScaleAnimate : MonoBehaviour {

	public float animSpeed;
	
	public int startScale;
	public int endScale;
	public float overflow = 1.1f;

	private int step = 0;
	private Vector2 oldScale;

	void Awake (){
		transform.localScale = new Vector2 (startScale, startScale);
		step = 0;
	}

	void Update (){

		switch (step) {
			case 0:
				oldScale = transform.localScale;
				transform.localScale = new Vector2 (oldScale.x + animSpeed, oldScale.y + animSpeed);
				if (transform.localScale.x > endScale * overflow) step = 1;
				break;
			case 1:
				oldScale = transform.localScale;
				transform.localScale = new Vector2 (oldScale.x - animSpeed, oldScale.y - animSpeed);
				if (transform.localScale.x < endScale ){
					transform.localScale = new Vector2 (endScale, endScale);
					step = 2;
				}
				break;
			case 2:				
				break;
			default:
				break;
		}
	}

}
