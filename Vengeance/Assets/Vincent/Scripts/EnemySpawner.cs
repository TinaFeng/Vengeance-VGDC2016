using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public Dictionary<string, GameObject> enemyDict;
	private bool spawningStarted;
	
	// Use this for initialization
	void Start () {
		enemyDict = new Dictionary<string, GameObject>();

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
		StartCoroutine(spawnEnemiesFromDictionary());
	}

	private IEnumerator spawnEnemiesFromDictionary() {
		Instantiate(enemyDict["EnemyPlaceholder"], transform.position, transform.rotation);
		yield return new WaitForSeconds(1.0f);
		Instantiate(enemyDict["EnemyPlaceholder2"], transform.position, transform.rotation);
		yield return new WaitForSeconds(1.0f);
		Instantiate(enemyDict["EnemyPlaceholder"], transform.position, transform.rotation);
	}
}
