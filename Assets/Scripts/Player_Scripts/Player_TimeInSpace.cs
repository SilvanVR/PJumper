using UnityEngine;
using System.Collections;
/*
This Script controls how long the Object is in Space (not in a Trigger-Collider of a Space-Object).
Based on timeToDieInSpace it kills the Object with the Star-Animation.
 */
public class Player_TimeInSpace : MonoBehaviour {
		
	public float timeToDieInSpace = 3;	//The Time the Player can be in Space (in Seconds)
	public float timeInSpace = 0;		//The Time the Player is in Space
	public float scaleDownSpeed = 0.95f; //The Scale-Down-Value (Multiplied every Frame with local Scale)
	public GameObject star;				//Star Prefab

	private bool inSpace = false;		//True: Player is in Space
	private bool calledOnce = false;	//True: The Coroutine "dieInSpace" was called
	private bool dying = false;			//True: Player begins to die
	
	// Update is called once per frame
	void Update () {	
		if (dying) {			
			if (scalePlayerDown()){	//Scale Player down (returns true if scale reached 0)			
				if (calledOnce) return;
				calledOnce = true;
				StartCoroutine(dieInSpace ());
			}
		}else if (inSpace){ 
			timeInSpace += Time.deltaTime; //add time
			if (timeInSpace > timeToDieInSpace){ //timeInSpace greater than TimeToDieInSpace?
				dying = true;	//Set dying to true, so he dies surely				
				GetComponent<Player_Movement> ().affectedByGravity = false; //Stop Gravity for this Player			
				GetComponent<Player_GunControl> ().deactiveGun (); //Deactive-Gun if Player has one
			}
		}
	}
	
	public void setInSpace (bool b){inSpace = b; if(b == false){timeInSpace = 0;}}	
	public bool getInSpace (){return inSpace;}
	public void setDying (bool b){GetComponent<Player_Movement> ().grounded = false; dying = b;} //called from Asteroid if Player is on it during Asteroid's Respawn
	public void resetScript() {timeInSpace = 0; calledOnce = false; dying = false;}

	/*Scale the Player down until Scale is 0, returns true then*/
	private bool scalePlayerDown (){
		if (transform.localScale.x == 0) return true;
		else{		
			transform.localScale = new Vector2 (transform.localScale.x * scaleDownSpeed, transform.localScale.y * scaleDownSpeed);
			if (Mathf.Abs (transform.localScale.x) < 0.1f) transform.localScale = Vector2.zero;
			return false;
		}
	}	
	
	/*Called from this Script if timeToDieInSpace > timeInSpace*/
	private IEnumerator dieInSpace (){
		GetComponent<Player_Movement> ().stopMove ();			//Stop Player-Moving
		Instantiate (star, transform.position, Quaternion.identity);
		yield return new WaitForSeconds (2);
		GetComponent<Player_Health> ().kill (); //Kill Player
	}
}
