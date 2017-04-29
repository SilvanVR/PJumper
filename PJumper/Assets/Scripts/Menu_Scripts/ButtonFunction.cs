using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonFunction : MonoBehaviour {

	public Button startButton;

	void Start (){
	}

	void startLevel (int level) {
		Application.LoadLevel (level);
	}

}
