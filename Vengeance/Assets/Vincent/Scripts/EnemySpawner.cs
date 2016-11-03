using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public List<GameObject> enemyTypes; 
	private bool spawningStarted;
	
	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			if (child.tag == "Enemy") {
				enemyTypes.Add(child.gameObject);
			}
		}
		spawningStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawningStarted) {
			startSpawning();
			spawningStarted = true;
		}
	}

	private void startSpawning() {
		StartCoroutine(spawnEnemyOncePerSecondCoroutine());
	}

	private IEnumerator spawnEnemyOncePerSecondCoroutine() {
		int flip = 1;
		for (int i = 0; i < 3; ++i) {
			Instantiate(enemyTypes[flip], transform.position, transform.rotation);
			Debug.Log("Spawned enemy " + i);
			flip = 1 - flip;
			yield return new WaitForSeconds(1.0f);
		}
	}
}
