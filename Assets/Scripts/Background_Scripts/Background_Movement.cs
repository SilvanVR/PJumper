using UnityEngine;
using System.Collections;

public class Background_Movement : MonoBehaviour {


	public float speed;			//Speed of the Background-Image
	public SpriteRenderer sprite; //The Sprite Renderer of the Background-Image

	private float bg_width;	//Width of the Background-Image

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (speed * Time.deltaTime, 0, 0);

		if (transform.localPosition.x < -bg_width) {		//Is the first Background-Image outside the Screen?
			transform.Translate (bg_width,0,0);
		}
	}

	public void setImageWidth (float width){
		bg_width = width;
	}

}
