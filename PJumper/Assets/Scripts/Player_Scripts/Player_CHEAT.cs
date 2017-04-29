using UnityEngine;
using System.Collections;

/*
 * Teleports the Player to the clicked - Mouse-Position
 */
public class Player_CHEAT : MonoBehaviour {

	public Canvas shop; //Disable this Cheat if Shop is open

	private bool cheatEnabled = false;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Delete)) cheatEnabled = true;
		if (cheatEnabled && Input.GetMouseButtonDown(0) && !shop.enabled) {
			Player_Movement player = GetComponent<Player_Movement> ();
			player.releasePlayerFromCurSpaceObject ();
			player.setDirToZero ();
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
