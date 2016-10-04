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

	//Used for combat calcs
	public GameObject punchEffect;

	// Use this for initialization
	void Start () {
		maxHealth = 100;
		currentHealth = 100;
		enemyName = "D E A D M E A T";
	}
	
	// Update is called once per frame
	void Update () {
		nameDisplay.text = enemyName;
		healthBar.fillAmount = currentHealth / maxHealth;
	}

	void OnCollisionEnter(Collision col){

		//When struck by the player attack, lowers health and spawns particle effect
		if (col.gameObject.layer == 8) {
			currentHealth -= 10f;
			Instantiate (punchEffect, col.gameObject.transform.position, Quaternion.identity);
		}
	}
}
