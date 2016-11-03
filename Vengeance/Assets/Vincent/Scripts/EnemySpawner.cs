﻿using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public EnemyPlaceholder enemyType; 
	private bool spawningStarted;
	
	// Use this for initialization
	void Start () {
		// TODO: should have abstract EnemyBase type, or is GameObject good enough?
		enemyType = GetComponentInChildren<EnemyPlaceholder>();
		spawningStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!spawningStarted) {
			StartCoroutine(spawnEnemyOncePerSecondCoroutine());
			spawningStarted = true;
		}
	}

	private IEnumerator spawnEnemyOncePerSecondCoroutine() {
		for (int i = 0; i < 3; ++i) {
			Debug.Log("Instantiating enemy " + i);
			Instantiate(enemyType, transform.position, transform.rotation);
			yield return new WaitForSeconds(1.0f);
		}
	}
}
