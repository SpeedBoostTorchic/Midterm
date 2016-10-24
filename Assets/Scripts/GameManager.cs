using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int numEnemies = 0;
	public int enemyCount = 0;
	public int enemiesKilled = 0;

	private int spawnUsed = 0;

	public GameObject enemy;

	public Transform spawnPoint1;
	public Transform spawnPoint2;
	public Transform spawnPoint3;

	public bool knifeSpawned = false;
	
	// Update is called once per frame
	void Update () {

		if (enemyCount == 0) {
			spawnFoe (1);
		}	
		if (enemiesKilled >= 1 && numEnemies < 2) {
			spawnFoe (3);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {

		}

		if (enemiesKilled > 15) {
			Application.LoadLevel (2);
		}
	}

	public void spawnFoe(int numSpawn){

		for (int i = 0; i < numSpawn; i++) {
			numEnemies++;
			enemyCount++;
			spawnUsed = Random.Range (0, 4);

			if (spawnUsed <= 1)
				Instantiate (enemy, spawnPoint1.position, Quaternion.identity);

			if (spawnUsed == 2)
				Instantiate (enemy, spawnPoint2.position, Quaternion.identity);

			if (spawnUsed >= 3)
				Instantiate (enemy, spawnPoint3.position, Quaternion.identity);

		}
	}
}
