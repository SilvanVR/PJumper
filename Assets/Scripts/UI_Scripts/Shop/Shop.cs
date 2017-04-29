using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public KeyCode showShopButton;
	public Player_Inventory player_inventory;
	public Player_Health player_health;
	public Player_GunControl gunControl;
	public Button buyDoubleJumpButton;
	public Button buyGunButton;
	public Button[] buyCoinButtons;

	private static Canvas shop;	
	private bool enoughCoins = false;
	private static bool gameOver;

	void Awake (){
		shop = GetComponent<Canvas> ();
		shop.enabled = false;
		gameOver = false;
	}

	void Update () {
		if (Input.GetKeyDown (showShopButton) && !gameOver) {
			triggerShop ();		
		}
		if (Input.GetKeyDown (KeyCode.Delete)) {
			for (int i = 0; i < buyCoinButtons.Length;i++){
				buyCoinButtons[i].interactable = true;
			}		
		}
	}

	public void addLifes (int lifes){		
		if (enoughCoins){
			player_health.addLifes (lifes);	
			enoughCoins = false;
		}
	}

	public void removeCoins (int numOfCoins){
		enoughCoins = player_inventory.removeCoins (numOfCoins);
	}

	public void enableDoubleJump (){
		if (enoughCoins){
			player_inventory.gameObject.GetComponent<Player_Movement> ().enableDoubleJump ();
			enoughCoins = false;
			buyDoubleJumpButton.interactable = false;
		}
	}

	public void addCoins (int numOfCoins){
		player_inventory.addItem (Item.COIN, numOfCoins);
	}

	public void addPlayerGun (){
		if (!enoughCoins) return;
		buyGunButton.interactable = false;
		gunControl.addGun ();
	}

	public static void disableShop (){
		shop.enabled = false;
		gameOver = true;
	}


	public void triggerShop (){
		shop.enabled = !shop.enabled;
	}
}
