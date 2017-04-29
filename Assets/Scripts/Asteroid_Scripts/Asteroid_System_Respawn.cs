using UnityEngine;
using System.Collections;

public class Asteroid_System_Respawn : MonoBehaviour {

	public Vector2 direction;			//The Direction Vector
	public float maxRange = 15;			//The Max-Range the Asteroid can move until he will respawn
	public float scaleDownSpeed = 0.95f; //The Scale-Down-Value (Multiplied every Frame with local Scale)
	public GameObject star;				//Star Prefab

	private float range = 0;
	private Vector2 spawnPoint;
	private bool calledOnce = false;
	
	// Use this for initialization
	void Start () {
		direction *= 0.1f;
		spawnPoint = transform.position;
	}
	
	void FixedUpdate () {
		if (calledOnce) return;			//Calculate Nothing if Asteroid begins to Respawn

		transform.position += (Vector3)direction;			//Adds the direction-Vector to the Position
		range += direction.magnitude;					//Calculate the covered Range

		if (range > maxRange) {			
			setPlayerParent();
			if (scaleAsteroidsDown ()){
				calledOnce = true;
				for (int i = 0; i < transform.childCount; i++) {
					Debug.Log (transform.GetChild(i).transform.position);
					StartCoroutine(destroyAsteroids (transform.GetChild(i).transform.position)); //Instantiate the Star-Prefab + Respawn after it
				}
			}		
		}	
	}

	
	/*Called from this Script if range > maxrange and scale = 0*/
	private IEnumerator destroyAsteroids (Vector2 position){
			Instantiate (star, position, Quaternion.identity);
			yield return new WaitForSeconds (3);
			transform.localScale = Vector2.one;					//Reset Scale
			range = 0;											//Reset Range
			calledOnce = false;									//Reset calledOnce
			transform.position = spawnPoint;					//Reset Position
			//direction = oldDirection;							//Reset the Direction (could be modified if Asteroid collided with, possibly, a Planet)
	}

	/*Scale the Asteroid down until Scale is 0, returns true then*/
	private bool scaleAsteroidsDown (){
		if (transform.GetChild (0).transform.localScale.x <= 0) return true;
		else{		
			for (int i = 0; i < transform.childCount; i++) {
				Vector3 childScale = transform.GetChild (i).transform.localScale;
				transform.GetChild (i).transform.localScale = new Vector3 (childScale.x * scaleDownSpeed, childScale.y * scaleDownSpeed,1);
			}
			//if (Mathf.Abs (transform.GetChild (0).transform.localScale.x) < 0.1f) transform.localScale = Vector3.zero;			
			return false;
		}
	}

	//Set the Player's Parent to the "PlayerParent-GameObject" if Asteroid begins to Respawn (Otherwise it scales the Player too)
	private void setPlayerParent (){
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		for (int i = 0; i < transform.childCount; i++) {
			if (player.GetComponent<Player_Movement> ().getCurrentSpaceObject () == transform.GetChild (i).gameObject){
				player.transform.SetParent (GameObject.Find("PlayerParent").transform);
			}
		}
	}

}
