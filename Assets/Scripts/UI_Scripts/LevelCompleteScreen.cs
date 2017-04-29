using UnityEngine;
using System.Collections;

public class LevelCompleteScreen : MonoBehaviour {
	
	public float firstTime = 0.2f;
	public float secondTime = 0.2f;
	
	public GameObject levelCompleteText;
	public GameObject nextLevelButton;
	public GameObject backToMenuButton;

	void Start (){
		nextLevelButton.SetActive (false);
		backToMenuButton.SetActive (false);
		StartCoroutine ("levelCompleteScreen");
	}

	IEnumerator levelCompleteScreen (){
		yield return new WaitForSeconds (firstTime);
		enableGameObject (nextLevelButton);
		yield return new WaitForSeconds (secondTime);
		enableGameObject (backToMenuButton);
	}
	
	private void enableGameObject (GameObject g){
		g.SetActive (true);
	}

}
