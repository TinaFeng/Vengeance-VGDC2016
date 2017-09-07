using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public enum BGMFlag { NONE, START_MUSIC, STOP_MUSIC, CHANGE_MUSIC }

public class EnemySpawner : MonoBehaviour {

	public float startDelay;
	public float[] spawnPlan;
	private BGMPlayer bgmPlayer;
    int i = 0;

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
		foreach (Transform child in transform) {
            if (i < spawnPlan.Length)
            {
                if (spawnPlan[i] > 0.0f)
                {
                    yield return new WaitForSeconds(spawnPlan[i]);
                }
                child.gameObject.SetActive(true);
                i++;
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