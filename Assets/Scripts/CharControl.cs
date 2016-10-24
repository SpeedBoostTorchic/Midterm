using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharControl : MonoBehaviour {

	public CharacterController cController;
	public Camera maincam; //Main Camera

	//Assets used for punching the enemy
	public GameObject fist;
	public GameObject player;
	public Rigidbody fistrb;

	float jumpTimer = 0; //used for Jump calculations
	float acc  = 5f; // speed up movement/mouselook by this amound

	public bool punching = false;
	public bool hasKnife = false;

	//These are used for the First Person assets
	public Image hand;
	public Sprite fistIdle;
	public Sprite fistPunch;
	public Sprite knifeIdle;
	public Sprite knifePunch;

	//Variables used for knockback
	private Vector3 knockDir = new Vector3 (0f,0f,0f);
	private bool knockback = false;
	private float knockForce = 0f;

	public CameraController cam;
	public GameObject dmgObject;
	public ScreenFader dmgFader;

	public GameManager GM;

	private Vector3 camHigh;
	private Vector3 camLow;
	private Vector3 camDefault;
	private bool camRising = true;

	//Audio related stuff
	public AudioSource playerSource;
	public AudioSource powerEffectSource;

	public AudioClip hurt;
	public AudioClip attack;
	public AudioClip powerUp;
	public AudioClip powerDown;
	bool hasPlayed = false;



	void Start () {
		cController = GetComponent<CharacterController> ();
		hand.sprite = fistIdle;

		Screen.lockCursor = true;
		camHigh = cam.transform.position += new Vector3 (0, 0.2f, 0);
		camLow = cam.transform.position -= new Vector3 (0, 0.5f, 0);
		camDefault = cam.transform.position + new Vector3 (0,0.3f,0);
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
		}

		if (Input.GetMouseButtonDown (0) && punching == false) {
			
			fistrb.AddForce(maincam.transform.forward*500);
			jumpTimer = Time.time + 0.25f;
			punching = true;
			hasPlayed = false;
		}
			

		if (Time.time > jumpTimer && punching == true) {
			fistrb.velocity = Vector3.zero;
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);
			punching = false;
		}

		if (punching == false) {
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);
		}

		if (!punching) {
			if (hasKnife) {
				hand.sprite = knifeIdle;
			} else {
				hand.sprite = fistIdle;
			}
		} else if (punching) {
			if (hasKnife) {
				hand.sprite = knifePunch;
			} else {
				hand.sprite = fistPunch;
			}

			if (!playerSource.isPlaying && !hasPlayed) {
				playerSource.clip = attack;
				playerSource.Play ();
				hasPlayed = true;
			}
		}

		if (knockback && knockForce > 0) {
			cController.SimpleMove (knockDir * knockForce);
			knockForce -= 10 * Time.deltaTime;

			if (!playerSource.isPlaying) {
				playerSource.clip = hurt;
				playerSource.Play ();
			}
		}

		//Determines if camera should sway up or down
		if (cam.transform.position.y >= camHigh.y)
			camRising = false;
		if (cam.transform.position.y <= camLow.y)
			camRising = true;

		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			if (camRising) {
				cam.transform.position += new Vector3 (0, 2f * Time.deltaTime, 0);
			}
			if (!camRising) {
				cam.transform.position -= new Vector3 (0, 2f * Time.deltaTime, 0);
			}
		} else {
			cam.transform.position = new Vector3(cam.transform.position.x,camDefault.y,cam.transform.position.z);
		}

	} //End Update()

	void OnCollisionEnter(Collision col){



		//When struck by the enemy attack, knocks back
		if (col.gameObject.layer == 11) {

			//Knocks back
			knockDir = col.gameObject.transform.forward;
			knockForce = 10f;
			knockback = true;

			//shakes the camera
			cam.ShakeCamera (0.8f, 36, 20, 20, 20);

			//activates screen fader when hit
			dmgObject.SetActive (true);
			dmgFader.currentAlpha = 1;
			dmgFader.fadingIn = true;

			if (hasKnife) {
				hasKnife = false;

				if (!powerEffectSource.isPlaying) {
					powerEffectSource.clip = powerDown;
					powerEffectSource.Play ();
				}
			}
		}

		if (col.gameObject.layer == 12){
			Debug.Log ("Touched knife!");
			hasKnife = true;

			GM.knifeSpawned = false;
			Destroy (col.gameObject);

			if (!powerEffectSource.isPlaying) {
				powerEffectSource.clip = powerUp;
				powerEffectSource.Play ();
			}
		}

	}

	void OnTriggerEnter(Collider col){


		if (col.gameObject.layer == 12){
			Debug.Log ("Touched knife!");
			hasKnife = true;
			GM.knifeSpawned = false;
			Destroy (col.gameObject);

			if (!powerEffectSource.isPlaying) {
				powerEffectSource.clip = powerUp;
				powerEffectSource.Play ();
			}

		}
	}
}
