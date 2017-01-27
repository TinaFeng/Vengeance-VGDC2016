﻿using UnityEngine;
using System.Collections;

public class TEMP_GameManager : MonoBehaviour {

	public BGMPlayer bgmPlayer;

	// Use this for initialization
	void Start () {
		bgmPlayer = GetComponentInChildren<BGMPlayer>();
		TEST_wait_startMusic_wait_stopMusic();
	}

	// Update is called once per frame
	void Update () {
	}

	private void TEST_wait_startMusic_wait_stopMusic() {
		StartCoroutine(waitCoroutine(2.0f));
	}

	private IEnumerator waitCoroutine(float seconds) {
		//yield return new WaitForSeconds(seconds);
		Debug.Log("calling bgmPlayer.start()");
		bgmPlayer.start();
		yield return new WaitForSeconds(seconds);
		bgmPlayer.changeTrack(1);
		// Debug.Log("calling bgmPlayer.stop()";
		// bgmPlayer.stop();
	}


}