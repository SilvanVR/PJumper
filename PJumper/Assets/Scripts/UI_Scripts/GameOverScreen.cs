using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	public float firstTime = 0.2f;
	public float secondTime = 0.2f;

	public GameObject gameOverText;
	public GameObject restartButton;
	public GameObject backToMenuButton;

	void Start (){
		restartButton.SetActive (false);
		backToMenuButton.SetActive (false);
		StartCoroutine ("gameOverScreen");
	}

	IEnumerator gameOverScreen (){
		yield return new WaitForSeconds (firstTime);
		enableGameObject (restartButton);
		yield return new WaitForSeconds (secondTime);
		enableGameObject (backToMenuButton);
	}

	private void enableGameObject (GameObject g){
		g.SetActive (true);
	}

}
