using UnityEngine;
using System.Collections;

public class BGMPlayer_TimeBased : MonoBehaviour {

	public int numAudioSources = 2;
	public BGMTrack_TimeBased track;
	public float volume = 1.0F;
	public double startDelay = 0.0F;

	private AudioSource[] audioSources;
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

		track = GetComponentInChildren<BGMTrack_TimeBased>();

		nextEventTime = AudioSettings.dspTime + startDelay;
	}
	
	// Update is called once per frame
	void Update () {
		double time = AudioSettings.dspTime;

		if (time + 1.0F > nextEventTime) {
			BGMClip bgmClip = track.getNextClip();
			audioSources[flip].clip = bgmClip.clip;
			audioSources[flip].PlayScheduled(nextEventTime);
			nextEventTime += bgmClip.lengthInSeconds;
			flip = 1 - flip;
		}
	}

	// NOTE: length of the fadein will not match lengthInSeconds exactly
	public void fadeIn(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		StartCoroutine(fadeInCoroutine(lengthInSeconds, interval));
	}

	// NOTE: length of the fadeout will not match lengthInSeconds exactly
	public void fadeOut(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		StartCoroutine(fadeOutCoroutine(lengthInSeconds, interval));
	}

	private IEnumerator fadeInCoroutine(float lengthInSeconds, float interval) {
		while (currentVolume < volume) {
			foreach (var source in audioSources) {
				source.volume += interval;
			}
			currentVolume += interval;		
			yield return new WaitForSeconds(lengthInSeconds * interval);
		}
	}

	private IEnumerator fadeOutCoroutine(float lengthInSeconds, float interval) {
		while (currentVolume > 0.0F) {
			foreach (var source in audioSources) {
				source.volume -= interval;
			}
			currentVolume -= interval;		
			yield return new WaitForSeconds(lengthInSeconds * interval);
		}
	}

}

