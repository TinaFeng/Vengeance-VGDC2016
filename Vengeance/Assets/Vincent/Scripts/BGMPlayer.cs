using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public int numAudioSources = 2;
	public BGMTrack[] tracks;
	public int startingTrackIndex = 0; // default: play the first BGMTrack in the list unless otherwise specified
	public float volume = 1.0F;
	public double startDelay = 0.0F;
	public bool startOnCreation = true; // default: start BGM playback when the Player is created

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

		// add children to list of BGMTracks
		tracks = GetComponentsInChildren<BGMTrack>();

		currentTrack = tracks[startingTrackIndex];

		if (startOnCreation) {
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

	// if there is another track currently playing, the player first stops that track.
	// There will be a short pause between stopping the currently playing track and starting the new one
	public void start(int trackIndex = 0) {
		currentTrack = tracks[trackIndex];
		isPlaying = true;
		nextEventTime = AudioSettings.dspTime + startDelay;
		Debug.Log("BGMPlayer started");
	}

	public void stop(bool smoothStop = true) {
		isPlaying = false;
		foreach (AudioSource source in audioSources) {
			if (smoothStop) {
				fadeOut(0.06f);
			} else {
				source.Stop();
			}
		}
		Debug.Log("BGMPlayer stopped");
	}

	public void changeTrack(int index) {
		if (isPlaying) {
			stop();
		}
		currentTrack = tracks[index];
		Debug.Log("queued track: " + currentTrack.name + " in BGMPlayer");
		start();
	}

	// I hate this but I can't think of a better way to do it at the moment
	private IEnumerator changeTrackCoroutine(int newTrackIndex) {
		// fast fadeout
		while (currentVolume > 0.0F) {
			foreach (var source in audioSources) {
				source.volume -= 0.05f;
			}
			currentVolume -= 0.05f;		
			yield return new WaitForSeconds(0.06f * 0.05f);
		}
		// stop audioSources and reset volumes
		foreach (var source in audioSources) {
			source.Stop();
			source.volume = volume;
		}
		// change currentTrack to the new track
		currentTrack = tracks[newTrackIndex];
		// start playback again
		start();
	}

	// NOTE: length of the fadein will not match lengthInSeconds exactly
	public void fadeIn(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		StartCoroutine(fadeInCoroutine(lengthInSeconds, interval));
	}

	// NOTE: length of the fadeout will not match lengthInSeconds exactly
	public void fadeOut(float lengthInSeconds = 1.0F, float interval = 0.05F) {
		StartCoroutine(fadeOutCoroutine(lengthInSeconds, interval));
	}

	public void resetVolume() {
		foreach (var source in audioSources) {
			source.volume += volume;
		}
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

