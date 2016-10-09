using UnityEngine;
using System.Collections;

public class CharControl : MonoBehaviour {

	public CharacterController cController;
	public Camera maincam; //Main Camera

	public GameObject fist;
	public GameObject player;
	public Rigidbody fistrb;

	float jumpTimer = 0; //used for Jump calculations
	float acc  = 5f; // speed up movement/mouselook by this amound

	public bool punching = false;


	void Start () {
		cController = GetComponent<CharacterController> ();
		//fist.SetActive (false);

	}

		
	void Update () {
		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");
		float mouseX = Input.GetAxis ("Mouse X"); //Currnet horizontal MouseSpeed
		float mouseY = Input.GetAxis ("Mouse Y");

		cController.SimpleMove (transform.forward * inputY * acc);
		cController.SimpleMove (transform.right * inputX * acc);
		transform.Rotate (0f, mouseX * acc, 0f);

		maincam.transform.Rotate (mouseY * -acc, 0f, 0f);

		if (maincam.transform.eulerAngles.x > 85 && maincam.transform.eulerAngles.x < 280) {
			maincam.transform.Rotate (mouseY * acc, 0f, 0f);
			Debug.Log (maincam.transform.eulerAngles.x);
		}

		if (Input.GetMouseButtonDown (0) && punching == false) {
			
			fistrb.AddForce(maincam.transform.forward*500);
			jumpTimer = Time.time + 0.25f;
			punching = true;
		}
			

		if (Time.time > jumpTimer && punching == true) {
			fistrb.velocity = Vector3.zero;
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);
			punching = false;
		}

		if (punching == false) {
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);
		}

	} //End Update()
}
