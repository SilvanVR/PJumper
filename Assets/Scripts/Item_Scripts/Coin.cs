using UnityEngine;
using System.Collections;

/*
 * Script for a Coin.
 */

public class Coin : Item {

	public int value; //Value for a Coin

	//Coin can collected by a Player
	void OnTriggerEnter2D (Collider2D other){
		if (other.CompareTag ("Player")) {
			other.GetComponent<Player_Inventory>().addItem (Item.COIN,value);
			Destroy (gameObject);
		}
	}
}
