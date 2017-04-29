using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * This Script animates the Text-Component (used by Player_Checkpoints to Display "Checkpoint reached").
 */
public class Text_Mid : MonoBehaviour {

	public float animSpeed;

	public int start_fontSize;
	public int end_fontSize;

	public float timeTillDestroy = 2f;
	public float fadeOutSpeed = 1;

	private Text text;
	private float startTime;
	private bool textOnScreen = true;

	void Awake () {
		text = GetComponent<Text> ();
	}

	void Update () {
		if (!textOnScreen) return;
		if (text.fontSize > end_fontSize){
			text.fontSize = (int)(text.fontSize - animSpeed);
			startTime = Time.time;
		}else{
			if(Time.time - startTime > timeTillDestroy){
				if (text.color.a > 0){		//Alpha is greater than 0? -> Than fade out the Text based on "fadeOutSpeed".
					Color col = text.color;
					col.a -= Time.deltaTime * fadeOutSpeed;
					text.color = col;
				}else{
					textOnScreen = false;
				}
			}
		}
	}

	public void newText (string str){
		textOnScreen = true;
		Color col = text.color;
		col.a = 1;
		text.color = col;
		text.fontSize = start_fontSize;
		text.text = str;
	}
}
