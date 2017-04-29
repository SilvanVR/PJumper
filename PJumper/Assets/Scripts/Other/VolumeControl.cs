using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {

	public Slider slider;
	public AudioSource music;
	public float disableTimer = 3f;
	
	private float startTime;
	
	void Update (){		
		if (slider.value != music.volume) startTime = Time.time; //Reset Timer

		music.volume = slider.value;
		if (Time.time - startTime > disableTimer) {
			gameObject.SetActive (false);		
		}
	}
	
	public void triggerSlider (){
		startTime = Time.time;
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
