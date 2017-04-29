using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Script controls the UI-Behaviour of the Coin-Management.
 */
public class Coin_ValueText : MonoBehaviour {

	public float fadeOutSpeed = 1;					//Fade-Out Speed. 
	public Text coinText;							//Text-Component for Displaying the Number of Coins

	private Text valueText;							//Text-Component for Displaying the Value of an Coin (automatically faded out)

	void Start () {
		valueText = GetComponent<Text> ();
	}

	void Update (){
		if (valueText.color.a > 0){		//Alpha is greater than 0? -> Than fade out the Text based on "fadeOutSpeed".
			Color col = valueText.color;
			col.a -= Time.deltaTime * fadeOutSpeed;
			valueText.color = col;
		}
	}

	/*
	 * Set's the UI-Text (total amount of Coins and the Value inside the Coin-Image).
	 * Reset's the Alpha and changes the color based on which Coin was picked up.
	 * Called from Player_Inventory.
	 */
	public void setText (int numOfCoins, int val){	
		coinText.text = "" + numOfCoins;	//Total amount of all Coins in Player-Inventory
		if (val > 0) valueText.text = "+" + val;			//Coin - Value
		else valueText.text = "" + val;
		if (val < 0) valueText.color = Color.black;
		else if (val <= Item.COIN1) valueText.color = Color.yellow;
		else if (val <= Item.COIN2) valueText.color = Color.green;
		else if (val <= Item.COIN3) valueText.color = Color.red;
		else if (val <= Item.COIN4) valueText.color = new Color (255,150,70); //Orange
 		else valueText.color = Color.white;

		Color col = valueText.color;		//Reset
		col.a = 1;							//the
		valueText.color = col;				//Alpha
	}
}