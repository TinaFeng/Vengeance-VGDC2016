using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	private AudioSource[] audioSources = new AudioSource[2];
	public BGMTrack track;

	private double nextEventTime;
	private int flip = 0;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < audioSources.Length; ++i) {
			GameObject child = new GameObject("MusicPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		nextEventTime = AudioSettings.dspTime + 2.0F;
	}
	
	// Update is called once per frame
	void Update () {
		double time = AudioSettings.dspTime;

		if (time + 1.0F > nextEventTime) {
			audioSources[flip].clip = track.getNextClip();
			audioSources[flip].PlayScheduled(nextEventTime);
			nextEventTime += 60.0F / track.bpm * track.beatsPerClip;
			flip = 1 - flip;
		}
	}
}
