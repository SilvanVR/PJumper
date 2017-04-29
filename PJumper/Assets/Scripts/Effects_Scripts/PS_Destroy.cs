using UnityEngine;
using System.Collections;

public class PS_Destroy : MonoBehaviour {

	public int secondsTilDestroy = 2;

	void Start () {
		Destroy (this.gameObject, secondsTilDestroy);
	}

}
