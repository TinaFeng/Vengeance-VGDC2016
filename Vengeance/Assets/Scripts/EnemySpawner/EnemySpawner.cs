using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

// ================
// || HOW TO USE ||
// ================
// 1. Add one instance of each enemy type to this spawner as a child object
// 2. Set enemy spawner settings
//		Start Spawning On Creation - execute spawn plan once this spawner is created
//			If this is set to false, enemies will not start spawning until startSpawning() is called by another script
//		Start Delay - how long to wait once this spawner is started before creating the first enemy
// 3. Set spawnPlan's length to the total number of enemes to spawn
// 4. Edit each entry in spawnPlan
// 		Enemy Type - the name of the child enemy game object to spawn
//		Position - coordinates to spawn it at
// 		Delay in Secs - how long to wait before spawning the enemy

public enum BGMFlag { NONE, START_MUSIC, STOP_MUSIC, CHANGE_MUSIC }

public class EnemySpawner : MonoBehaviour {

    [System.Serializable]
	public class EnemySpawnInfo {
        public GameObject enemy;
		public float delayInSecs;
		public BGMFlag bgmFlag = BGMFlag.NONE;
    }
	public float startDelay;
	public EnemySpawnInfo[] spawnPlan;
	private BGMPlayer bgmPlayer;
	
	// Use this for initialization
	void Start () {
		bgmPlayer = FindObjectOfType<BGMPlayer>();
        startSpawning();
	}

	public void startSpawning() {
		StartCoroutine(startSpawnPlan());
	}

	// All enemies are spawned with rotation Quaternion.identity for now
	// spawn.position uses absolute coordinates for now
	private IEnumerator startSpawnPlan() {
		if (startDelay > 0.0f) {
			yield return new WaitForSeconds(startDelay);
		}

		foreach (EnemySpawnInfo spawn in spawnPlan) {
            if (spawn.enemy != null) {
				if (spawn.delayInSecs > 0.0f) {
					yield return new WaitForSeconds(spawn.delayInSecs);
				}
                Debug.Log(true);
                spawn.enemy.SetActive(true);
            }
            if (spawn.bgmFlag != BGMFlag.NONE) {
				updateBGMPlayer(spawn.bgmFlag);
			}
		}
	}

	private void updateBGMPlayer(BGMFlag flag) {
		switch (flag) {
			case BGMFlag.START_MUSIC:
				bgmPlayer.start();
				break;
			case BGMFlag.STOP_MUSIC:
				bgmPlayer.stop();
				break;
			case BGMFlag.CHANGE_MUSIC:
				bgmPlayer.changeTrack(bgmPlayer.currentTrackIndex + 1);
				break;
			case BGMFlag.NONE:
			default:
				Debug.Log("Invalid flag passed to updateBGMPlayer(): " + flag.ToString());
				break;
		}
	}
}
