using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	//Used in the enemy health display
	public float maxHealth;
	public float currentHealth;
	public Image healthBar;
	public Text nameDisplay;
	public string enemyName;
	public string[] enemyNameList = { "D E A D M E A T", "P U S H O V E R", "W E A K L I N G", "V I L L A I N" };

	//Used for Enemy attack code
	float punchTimer = 0; //used for Jump calculations
	float punchStart = 0;
	bool punching = false;
	public GameObject fist;
	public Rigidbody fistrb;


	//Used for combat calcs
	public GameObject punchEffect;
	public GameObject stabEffect;
	public CharControl player;

	public Transform thisTransform;
	public Rigidbody thisRigidbody;
	public Transform playerTransform;

	//Movement calcuations
	private float turnSpeed = 10f;
	private float defaultMovement = 0;
	private float moveAcceleration = 10f;
	private float moveDrag = 0.8f;

	public GameManager GM;
	public Color uiColor  =  Color.magenta; 
	public GameObject knifePowerup;



	// Use this for initialization
	void Start () {
		maxHealth = 300;
		currentHealth = 300;
		enemyName = enemyNameList[Random.Range(0, enemyNameList.Length)];

		thisTransform = GetComponent<Transform>();
		thisRigidbody = GetComponent<Rigidbody>();

		player = GameObject.Find("Player").GetComponent<CharControl>();
		playerTransform = player.GetComponent<Transform> ();

		GM = GameObject.Find ("GameManager").GetComponent<GameManager>();


		switch (Random.Range(1,5)) {
		case 1: 
			uiColor = Color.cyan;
			break;

		case 2:
			uiColor = Color.yellow;
			break;
		case 3:
			uiColor = Color.green;
			break;

		case 4:
			uiColor = Color.magenta;
			break;

		default:
			uiColor = Color.magenta;
			break;
		}

		defaultMovement = Random.Range (9, 12);
		turnSpeed = Random.Range (7, 11);
	}

	void FixedUpdate()
	{
		// Skip if there's no player
		if (playerTransform == null || !playerTransform.gameObject.activeInHierarchy) return;

		// Rotate slowly toward the player's horizontal position
		var towardPlayer = playerTransform.position - thisTransform.position;
		towardPlayer.y = 0f;

		// Create a new rotation and interpolate toward it
		var newRotation = Quaternion.LookRotation(towardPlayer.normalized);
		thisRigidbody.rotation = Quaternion.Slerp(thisRigidbody.rotation, newRotation, Time.deltaTime * turnSpeed);

		// Stumble blindly forward
		thisRigidbody.AddForce(
			thisTransform.forward.x * moveAcceleration,
			0f,
			thisTransform.forward.z * moveAcceleration,
			ForceMode.Acceleration);		

		// Applies drag manually
			thisRigidbody.AddForce(
			-thisRigidbody.velocity.x * moveDrag,
			-Mathf.Pow(thisRigidbody.velocity.y, 2f) * Mathf.Sign(thisRigidbody.velocity.y),
			-thisRigidbody.velocity.z * moveDrag,
			ForceMode.Acceleration);
	}

	
	// Update is called once per frame
	void Update () {

		moveAcceleration = defaultMovement - Random.Range (1, 2);

		//Code handling the act of punching || Input.GetKeyDown (KeyCode.L) && punching == false
		if (Time.time > punchStart) {
			
			fistrb.AddForce (gameObject.transform.forward * 500);
			punchTimer = Time.time + 0.25f;
			punching = true;
			punchStart = Time.time + Random.Range (4, 8);
		}

		if (Time.time > punchTimer) {
			fistrb.velocity = Vector3.zero;
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);
			punching = false;
		}
		if (punching == false) {
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);

		} //End punching related code

		if (currentHealth <= 0) {
			
			if (player.hasKnife == false && !GM.knifeSpawned) {
				Instantiate (knifePowerup,this.transform.position, Quaternion.Euler(-90,0,0));
				GM.knifeSpawned = true;
			}
			GM.numEnemies--;
			GM.enemiesKilled++;
			Destroy (gameObject);

		}
	}

	void OnCollisionEnter(Collision col){

		//When struck by the player attack, lowers health and spawns particle effect
		if (col.gameObject.layer == 8) {
			currentHealth -= 20f;
			nameDisplay.text = enemyName;
			nameDisplay.color = uiColor;
			healthBar.fillAmount = currentHealth / maxHealth;
			healthBar.color = uiColor;

			if (player.hasKnife) {
				currentHealth -= 10f;
				Instantiate (stabEffect, col.gameObject.transform.position, Quaternion.identity);
			} else {
				Instantiate (punchEffect, col.gameObject.transform.position, Quaternion.identity);
			}
		}
	}
}
