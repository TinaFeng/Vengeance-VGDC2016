using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public int numAudioSources = 2;
	private AudioSource[] audioSources;
	public BGMTrack track;
	public float startingVolume = 1.0F;
	public double startDelay = 0.0F;

	private float currentVolume = 1.0F;
	private double nextEventTime;
	private int flip = 0;

	// Use this for initialization
	void Start () {
		audioSources = new AudioSource[numAudioSources];
		for (int i = 0; i < audioSources.Length; ++i) {
			GameObject child = new GameObject("MusicPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		nextEventTime = AudioSettings.dspTime + startDelay;
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

	// NOTE: length of the fadein will not match lengthInSeconds exactly
	public void fadeIn(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		Debug.Log(string.Format("Called fadeIn() with length {0} and interval {1}", lengthInSeconds, interval));
		StartCoroutine(fadeInCoroutine(lengthInSeconds, interval));
	}

	// NOTE: length of the fadeout will not match lengthInSeconds exactly
	public void fadeOut(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		Debug.Log(string.Format("Called fadeOut() with length {0} and interval {1}", lengthInSeconds, interval));
		StartCoroutine(fadeOutCoroutine(lengthInSeconds, interval));
	}

	private IEnumerator fadeInCoroutine(float lengthInSeconds, float interval) {
		double DEBUG_fadeoutTimer = AudioSettings.dspTime;
		Debug.Log(string.Format("fadeOut() began at time {0}", DEBUG_fadeoutTimer));

		while (currentVolume < startingVolume) {
			foreach (var source in audioSources) {
				source.volume += interval;
			}
			currentVolume += interval;		
			yield return new WaitForSeconds(lengthInSeconds * interval);
		}

		DEBUG_fadeoutTimer -= AudioSettings.dspTime;
		Debug.Log(string.Format("fadeOut() lasted {0} seconds", DEBUG_fadeoutTimer * -1));
	}

	private IEnumerator fadeOutCoroutine(float lengthInSeconds, float interval) {
		double DEBUG_fadeoutTimer = AudioSettings.dspTime;
		Debug.Log(string.Format("fadeOut() began at time {0}", DEBUG_fadeoutTimer));

		while (currentVolume > 0.0F) {
			foreach (var source in audioSources) {
				source.volume -= interval;
			}
			currentVolume -= interval;		
			yield return new WaitForSeconds(lengthInSeconds * interval);
		}
		
		DEBUG_fadeoutTimer -= AudioSettings.dspTime;
		Debug.Log(string.Format("fadeOut() lasted {0} seconds", DEBUG_fadeoutTimer * -1));
	}

}
