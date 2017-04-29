using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * This Script controls the whole Level. 
 * It controls the Main-Gravity, the Gravity-Radius of all Space-Objects.
 * It controls the Background-Music.
 * It controls the Number of Players and when the level will be restart.
 */
public class GameManager : MonoBehaviour {

	public static GameManager instance;		//The GameManager Object (only one possible)
	public static Camera_Follower cam;		//The Camera-Follower Script
	public static float main_Gravity = 0.02f; //Modify the Gravity for ALL Space-Objects
	public static float gravity_radius_multiply = 3; //Modify the Gravity-Radius for Space-Objects (Planets, Sun, Asteroids + BLACKHOLES)
	//public static float gravity_radius_multiply_blackhole = 3; //Modify the Gravity-Radius for all Black-Holes
	public static float gravity_size_strength = 0.15f; //Relation of SpaceObject-Size and Gravity-Strength (ONLY <1 ALLOWED) BIGGER: HIGHER DIFFERENCE 

	public static int maxPlayers = 2;
	public GameObject[] players; //The Active Players
	public int numPlayers;	//The Number of Active Players

	public Canvas[] uI_Slots; //The UI-Slot(s) for the Player(s)
	public GameObject gameOverScreen;
	public GameObject levelCompleteScreen;

	void Awake () {
		if (instance == null) {
			instance = this;		
		}else{
			Destroy (this);
		}
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera_Follower> ();
		players = new GameObject[maxPlayers];
		numPlayers = 0;
	}

	void Update () {
		if (Application.loadedLevel != 0) {
			if (Input.GetKeyDown (KeyCode.Escape)){
				backToMenu ();
			}		
		}
	}

	/*
	 * Add/Remove a Player from the Game. Game Over if numPlayers = 0.
	 */
	public void addPlayer (GameObject player){
		if (numPlayers == maxPlayers) return;
		players [numPlayers] = player;
		player.GetComponent<Player_UI_SlotControl> ().setSlot (uI_Slots [numPlayers]); //Give's the Player an UI-Slot
		numPlayers++;
		//Debug.Log ("Add Player called... new NumPlayer: " + numPlayers);
	}
	public void removePlayer (GameObject player){ 
		for (int i = 0; i < numPlayers; i++) {
			if (players[i] == player){				
				if (numPlayers != 1) players[i].GetComponent<Player_UI_SlotControl> ().disableSlot (); 	//Disable the UI-Slot
				players[i] = null;
			}
		}		
		numPlayers--;
		//Debug.Log ("Remove Player called... new NumPlayer: " + numPlayers);
		cam.notify (); //Notify the Cam that a Player died (To change the Target if nessecary)
		if (numPlayers == 0) gameOver ();
	}

	/*
	 * Actives/Deactives all Lifebars in the Game.
	 */
	public static void setEnemyLifeBars (bool b){
		GameObject[] healthbars = GameObject.FindGameObjectsWithTag ("Healthbar");
		foreach (GameObject hb in healthbars) {
			hb.GetComponent<Renderer>().enabled = b;
		}
	}
	
	public void gameOver (){gameOverScreen.SetActive (true); Shop.disableShop ();}
	public void startLevel (int level){Application.LoadLevel (level);}
	public void nextLevel (){
		int nextLevel = Application.loadedLevel + 1;
		if (nextLevel < Application.levelCount) {			
			Application.LoadLevel (nextLevel);
		}
	}
	public void restartLevel (){Application.LoadLevel (Application.loadedLevel);}
	public void backToMenu (){Application.LoadLevel (0);}
	public void levelComplete (){
		for (int i = 0; i < numPlayers; i++) {
			if (players[i] != null){				
				players[i].GetComponent<Player_Movement> ().disableJump ();	//Disable ability to Jump
			}
		}	
		levelCompleteScreen.SetActive (true);	//Activates the Level-Complete Screen
	}
	public void closeApplication () {Application.Quit ();}
	public void showCredits (){Credits.showCredits ();}
}
