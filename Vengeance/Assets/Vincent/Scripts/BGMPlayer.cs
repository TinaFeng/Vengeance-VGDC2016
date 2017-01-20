using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public int numAudioSources = 2;
	public BGMTrack[] tracks;
	public int startingTrackIndex = 0; // default: play the first BGMTrack in the list unless otherwise specified
	public float volume = 1.0F;
	public double startDelay = 0.0F;
	public bool startOnInit = true; // default: start BGM playback when the Player is created

	private AudioSource[] audioSources;
	private BGMTrack currentTrack;
	private float currentVolume = 1.0F;
	private double nextEventTime;
	private int flip = 0;
	private bool isPlaying = false;

	// Use this for initialization
	void Start () {
		audioSources = new AudioSource[numAudioSources];
		for (int i = 0; i < audioSources.Length; ++i) {
			GameObject child = new GameObject("MusicPlayer");
			child.transform.parent = gameObject.transform;
			audioSources[i] = child.AddComponent<AudioSource>();
		}

		tracks = GetComponentsInChildren<BGMTrack>();

		// add children to list of BGMTracks
		currentTrack = tracks[startingTrackIndex];

		if (startOnInit) {
			start();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!isPlaying) {
			return;
		}
		double time = AudioSettings.dspTime;

		if (time + 1.0F > nextEventTime) {
			BGMClip bgmClip = currentTrack.getNextClip();
			audioSources[flip].clip = bgmClip.clip;
			audioSources[flip].PlayScheduled(nextEventTime);
			nextEventTime += bgmClip.lengthInSeconds;
			flip = 1 - flip;
		}
	}

	public void start() {
		isPlaying = true;
		nextEventTime = AudioSettings.dspTime + startDelay;
		Debug.Log("BGMPlayer started");
	}

	public void stop(bool smoothStop = true) {
		isPlaying = false;
		foreach (AudioSource source in audioSources) {
			if (smoothStop) {
				fadeOut(0.1f);
			} else {
				source.Stop();
			}
		}
		Debug.Log("BGMPlayer stopped");
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

