using UnityEngine;
using System.Collections;

public class fillToFullScreen : MonoBehaviour {

	public GameObject secondImage;

	// Use this for initialization
	void Start () {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		
		float worldScreenHeight = Camera.main.orthographicSize * 2;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		transform.localScale = new Vector3(
			worldScreenWidth / sr.sprite.bounds.size.x,
			worldScreenHeight / sr.sprite.bounds.size.y, 1);

		if (secondImage == null) return;
		secondImage.transform.Translate (sr.sprite.bounds.size.x * transform.localScale.x,0,0);
		transform.parent.gameObject.GetComponent<Background_Movement> ().setImageWidth (sr.sprite.bounds.size.x * transform.localScale.x);
	}
	
	// Update is called once per frame
	void Update () {}

}
