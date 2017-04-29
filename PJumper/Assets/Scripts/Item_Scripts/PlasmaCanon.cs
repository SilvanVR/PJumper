using UnityEngine;
using System.Collections;

/*
 * The Script for the PlasmaCanon. Inherites from Item-Class.
 */
public class PlasmaCanon : Item
{
	//Canon can collected by a Player.
	void OnTriggerEnter2D (Collider2D other){
		if (other.tag == "Player") {
			other.GetComponent<Player_Inventory>().addItem (Item.PLASMACANON,1);
			Destroy (this.gameObject);
		}
	}
}

