using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	private static bool start = false;
	private static float animSpeed = 50;
	private static GameObject StandardUI;
	private static Vector2 startPosition;
	public GameObject lastText;

	void Start () {
		StandardUI = GameObject.Find ("Standard");
		transform.position = new Vector2 (transform.position.x, 0);
		startPosition = transform.position;
	}
	

	void Update () {
		if (!start) return;
		Vector2 up = Vector2.up * Time.deltaTime * animSpeed;
		transform.Translate (up);
		if (lastText.transform.position.y - lastText.GetComponent<RectTransform>().rect.height > Screen.height || Input.GetMouseButtonDown (0)) {			
			transform.position = startPosition;
			enableStandardUI ();
		}
	}

	public static void showCredits (){
		start = true;
		StandardUI.SetActive (false);
	}

	private static void enableStandardUI (){
		start = false;
		StandardUI.SetActive (true);
	}
}
