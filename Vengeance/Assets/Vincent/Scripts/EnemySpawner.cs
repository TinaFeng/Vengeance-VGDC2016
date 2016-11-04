using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	[System.Serializable]
	public class EnemySpawnInfo {
		public string enemyType;
		public Vector3 position;
		public float delayInSecs;

	}

	public float enemySpawnerStartDelay;
	public Dictionary<string, GameObject> enemyDict;
	public EnemySpawnInfo[] spawnPlan;
	private bool spawningStarted;
	
	// Use this for initialization
	void Start () {
		enemyDict = new Dictionary<string, GameObject>();

		// enemyDict only adds children GameObjects if they have the "Enemy" tag
		foreach (Transform child in transform) {
			if (child.tag == "Enemy") {
				Debug.Log("Added " + child.gameObject.name);
				enemyDict.Add(child.gameObject.name, child.gameObject);
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
		StartCoroutine(startSpawnPlan());
	}

	// All enemies are spawned with rotation Quaternion.identity for now
	// spawn.position uses absolute coordinates for now
	private IEnumerator startSpawnPlan() {
		if (enemySpawnerStartDelay > 0.0f) {
			yield return new WaitForSeconds(enemySpawnerStartDelay);
		}

		foreach (EnemySpawnInfo spawn in spawnPlan) {
			Debug.Log("Spawning " + spawn.enemyType + " with delay " + spawn.delayInSecs);
			if (spawn.delayInSecs > 0.0f) {
				yield return new WaitForSeconds(spawn.delayInSecs);
			}
			Instantiate(enemyDict[spawn.enemyType], spawn.position, Quaternion.identity);
		}
	}
}
