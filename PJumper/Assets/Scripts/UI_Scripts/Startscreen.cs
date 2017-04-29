using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Startscreen : MonoBehaviour {

	public Image blackscreen;
	public Text text;

	public float fadeOutSpeedBlackscreen = 0.1f;
	public float fadeOutSpeedText = 0.1f;

	public float waitTime = 2f;

	private float startTime;

	void Start (){
		startTime = Time.time;
	}
			
	void Update () {
		if (Time.time - startTime < waitTime) return;
		if (blackscreen.color.a > 0){		//Alpha is greater than 0? -> Than fade out the Text based on "fadeOutSpeed".
			Color col = blackscreen.color;
			col.a -= Time.deltaTime * fadeOutSpeedBlackscreen;
			blackscreen.color = col;

			Color col2 = text.color;
			col2.a -= Time.deltaTime * fadeOutSpeedText;
			text.color = col2;
		}else {
			Destroy (this.gameObject);
		}
	}
}
