using UnityEngine;
using System.Collections;

/*
 * This Script animates an Blackhole (Animated Size over Time).
 */
public class Blackhole_Animate : MonoBehaviour {

	public float maxScale;		//The Max-Scale
	public float minScale;		//The Min-Scale

	public float timeMinScale;		//The Time in which the Blackhole will be stay at Min-Scale
	public float timeMaxScale;		//The Time in which the Blackhole will be stay at Max-Scale

	public float animSpeed = 1;		//The Animation-Speed

	public Sprite supernova;		//Supernova-Sprite (2D-Image)
	public Sprite blackhole;		//Normal Blackhole-Sprite (2D Image)

	private int state = 0;			//The current State of the Animation (changes dynamicaly)
	private float curScale;			//The current Scale of the Blackhole
	private float timer;			//The timer for switching the States
	private SpriteRenderer sr;		//Sprite-Renderer for changing the Sprite
	
	void Awake () {
		curScale = transform.localScale.x;	//Get current-Scale
		sr = GetComponent<SpriteRenderer> ();	//Set reference to Sprite-Renderer
		animSpeed *= 0.01f;
	}

	//Calculates the Animation
	void Update () {

		switch (state) {
			case 0:	//Blackhole will becomer taller and taller till maxScale is reached.
				curScale += animSpeed;
				scale (curScale);
				if (curScale >= maxScale){ 	//Max-Scale is reached
					state = 1;				//Next State
					timer = Time.time;		//Set Timer
					behaveAtMaxScale (); 	//Tells what to do if Blackhole is at Max-Scale
				}
				break;
			case 1: //The Wait time in Max-Scale
				if (Time.time - timer > timeMaxScale){ //Wait-Time is gone
					state = 2;					//Next State
					leavingMaxSCale ();
				}
				break;
			case 2: //Blackhole will become smaller and smaller till minScale is reached
				curScale -= animSpeed;
				scale (curScale);
				if (curScale <= minScale){	//Min-Scale is reached
					state = 3;				//Next State
					timer = Time.time;		//Set Timer
				}
				break;
			case 3:	//The Wait time in Min-Scale
				if (Time.time - timer > timeMinScale) state = 0;	//Restart the Animation
				break;
			default:
				Debug.LogError ("Blackhole-Animation: State is Wrong!!");
				break;
		}

	}

	//Behaviour at Max-Scale
	private void behaveAtMaxScale (){
		sr.sprite = supernova;	//Change Sprite
	}

	//Reset's the Behavior at Max-Scale
	private void leavingMaxSCale (){
		sr.sprite = blackhole;		//Change Sprite
	}

	private void scale (float scale){
		if (scale < 0) scale = 0;
		transform.localScale = new Vector2 (scale, scale);
	}

}
