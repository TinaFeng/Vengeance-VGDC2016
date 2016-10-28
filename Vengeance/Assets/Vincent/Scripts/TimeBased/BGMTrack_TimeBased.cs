﻿using UnityEngine;
using System.Collections;

public class BGMTrack_TimeBased : MonoBehaviour {

	public BGMClip[] clips; 

	private int currentClip = 0;
	private bool started = false;

	// Use this for initialization
	void Start () {
		started = false;
	}

	// returns clips[0] if the BGMTrack object has just been initialized
	// else, it returns the next clip in the array
	public BGMClip getNextClip() {
		if (!started) {
			currentClip = 0;
			started = true;
		} else {
			currentClip = (currentClip  + 1) % clips.Length;
		}
		return clips[currentClip];
	}

}
