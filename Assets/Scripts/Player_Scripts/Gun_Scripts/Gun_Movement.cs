using UnityEngine;
using System.Collections;

/*
 * This Script handle the speed of the Projectiles and the Lifetime.
 */
public class Gun_Movement : MonoBehaviour {

	public float maxlivetime = 2f;	//The Lifetime in Seconds

	private int damage;	//The amount of damage, the Bullet will do to an Enemy

	void Awake (){
		damage = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player_GunControl> ().getDamage ();
	}

	void Update () {
		Destroy (this.gameObject,maxlivetime);
	}

	void OnCollisionEnter2D (Collision2D other){
		if (other.gameObject.tag == "Enemy") {
			//Apply damage to the Enemy + hit-Position for "Blood-Effect"
			other.gameObject.GetComponent<Enemy_Health>().applyDamage (damage,(Vector2)transform.position); 
			Destroy (this.gameObject);	//Destroy the current Bullet
		}else if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<Player_Health>().applyDamage (damage); //Apply damage to the Player
			Destroy (this.gameObject); //Destroy Bullet
		}else{
			Destroy (this.gameObject); //Destroy Bullet
		}
	}

}
