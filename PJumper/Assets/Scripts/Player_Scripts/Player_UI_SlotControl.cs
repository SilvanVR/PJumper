using UnityEngine;
using System.Collections;

/*
 * Hold's the given Canvas (UI-Slot) from the GameManager and care about all Script's who need UI-Elements from this Slot.
 */
public class Player_UI_SlotControl : MonoBehaviour {

	private Canvas myUISlot;

	//Set's the UI-Slot for the Player - Called from the GameManager
	public void setSlot (Canvas mySlot){
		if (!mySlot.gameObject.activeSelf) mySlot.gameObject.SetActive (true);
		myUISlot = mySlot;

		UI_PlayerSlot script = myUISlot.GetComponent<UI_PlayerSlot> ();

		GetComponent<Player_Inventory> ().setCoinValueText(script.CoinValueText);
		GetComponent<Player_Health> ().setUIText(script.lifeText);
	}

	//Disable the UI-Slot (If a Player dies) - Called from the GameManager
	public void disableSlot (){
		myUISlot.gameObject.SetActive (false);
	}
}
