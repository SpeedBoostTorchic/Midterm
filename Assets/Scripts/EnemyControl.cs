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
	bool punching = false;
	public GameObject fist;
	public Rigidbody fistrb;

	//Used for combat calcs
	public GameObject punchEffect;

	// Use this for initialization
	void Start () {
		maxHealth = 100;
		currentHealth = 100;
		enemyName = enemyNameList[Random.Range(0, enemyNameList.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		nameDisplay.text = enemyName;
		healthBar.fillAmount = currentHealth / maxHealth;


		//Code handling the act of punching
		if (Input.GetKeyDown (KeyCode.L) && punching == false) {
			fistrb.AddForce (gameObject.transform.forward * 500);
			punchTimer = Time.time + 0.25f;
			punching = true;
		}

		if (Time.time > punchTimer) {
			fistrb.velocity = Vector3.zero;
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);
			punching = false;
		}
		if (punching == false) {
			fist.transform.localPosition = new Vector3 (0, 0.5f, 0);

		} //End punching related code
	}

	void OnCollisionEnter(Collision col){

		//When struck by the player attack, lowers health and spawns particle effect
		if (col.gameObject.layer == 8) {
			currentHealth -= 10f;
			Instantiate (punchEffect, col.gameObject.transform.position, Quaternion.identity);
		}
	}
}
