using UnityEngine;
using System.Collections;

public class ParticleControl : MonoBehaviour {

	//Takes the particle component of this G.O.
	private ParticleSystem thisEmission;

	// Use this for initialization
	void Start () {
		thisEmission = this.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (thisEmission.IsAlive() == false) {
			Destroy (gameObject);
		}
	}
}
