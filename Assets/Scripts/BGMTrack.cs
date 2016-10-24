using UnityEngine;
using System.Collections;

public class BGMTrack : MonoBehaviour {

	public string trackName;
	public AudioClip[] clips; // order of the AudioClips matters!
	public double bpm;
	public int numClips;
	public int beatsPerClip;

	private int currentClip;

	// Use this for initialization
	void Start () {
		currentClip = 0;
	}
	
/*
	// Update is called once per frame
	void Update () {
	
	}
*/

	// getNextClip returns the first clip if startFromBeginning is true
	// or the next clip in the list if false
	public AudioClip getNextClip(bool startFromBeginning = false) {
		if (startFromBeginning) {
			currentClip = 0;
			return clips[currentClip];
		}
		
		currentClip = (currentClip  + 1) % clips.Length;
		return clips[currentClip];
	}
}
