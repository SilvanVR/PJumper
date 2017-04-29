using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/*
 * The Inventory System for an Player. Realized through an Dictionary (Hashmap in JAVA) - Combines an String with an Integer (E.G. Coins - 100).
 * Holds a Reference to the coinValueObject which is responsible for the updating the UI.
 */
public class Player_Inventory : MonoBehaviour {

	public Dictionary<string,int> inventory; 	//Inventoy Dictionary -- Holds a Value for a String

	private Coin_ValueText coinValueText;		//Holds the Text-Component and is responsible for fading out the Text

	void Start () {
		inventory = new Dictionary<string,int> ();
	}

	/*
	 * Adds an (new) Item/s to the Dictionary.
	 */
	public void addItem (string it, int val){
		if (inventory.ContainsKey (it)) inventory[it] += val; //Player has already that Item, so add "val" to it.
		else inventory.Add (it,val); 						  //Add the new Item into the Dictionary.
		addKey (it, val);	//Call Private Function addKey, to specify what to do exactly with that Item (e.g. gun - calls addGun() in a Player-Script)
		//Debug.Log ("Key: " + it + " Num: " + inventory [it]);
	}

	/*
	 * Remove the given Item from the Dictionary. 
	 */
	public void removeItem (string it){
		if (inventory.ContainsKey (it)) inventory.Remove (it);
		removeKey (it, 0);
	}

	/*
	 * Remove a certain amount of the Item from the Dictionary. Removes the Item if the amount is <= 0.
	 */
	public void removeItem (string it, int val){		
		if (inventory.ContainsKey (it)) { //Key is available?
			inventory[it] -= val;	//subtract val from it.
			if (inventory[it] < 0) inventory.Remove (it); //Remove Item completely if val <= 0
			removeKey (it, val);
		}	
		//if (inventory.ContainsKey(it)) Debug.Log ("Key: " + it + " Num: " + inventory [it]);
		//else Debug.Log ("Key nicht vorhanden");
	}

	public void setCoinValueText (Coin_ValueText script){
		coinValueText = script;
	}

	public bool removeCoins (int numOfCoins){
		if (!inventory.ContainsKey (Item.COIN) || inventory[Item.COIN] - numOfCoins < 0){
			return false;
		}else{
			removeItem (Item.COIN, numOfCoins);
			updateUI (-numOfCoins);
			return true;
		}
	}

	//-------------------------------------------- PRIVATE METHODS ---------------------------------------------------------

	/*
	 * Specify what to do for the added Item.
	 */
	private void addKey (string key, int val){
		if (key == Item.PLASMACANON) GetComponent<Player_GunControl> ().addGun ();	//Gun added - activate the Script in Player and add a Gun
		if (key == Item.COIN){														//Coin collected
			updateUI (val);
		}
	}

	/*
	 * Specify what to do for the removed Item.
	 */
	private void removeKey (string key, int val){
		if (key == Item.PLASMACANON) GetComponent<Player_GunControl> ().removeGun ();
		if (key == Item.COIN) updateUI (-val);
	}

	private void updateUI(int val){
		coinValueText.setText (inventory[Item.COIN],val);			//Update UI-text
	}


}
