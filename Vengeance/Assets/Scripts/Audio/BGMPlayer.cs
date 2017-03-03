using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public int numAudioSources = 2;
	public BGMTrack[] tracks;
	public int startingTrackIndex = 0; // default: play the first BGMTrack in the list unless otherwise specified
	public float volume = 1.0F;
	public bool startOnCreation = true; // default: start BGM playback when the Player is created

	private AudioSource[] audioSources;
	private BGMTrack currentTrack;
	public int currentTrackIndex; // ugly, but Invoke doesn't allow passing parameters and I don't want to be in Coroutine hell
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

		currentTrackIndex = startingTrackIndex;
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

	public void reset() {
		foreach (BGMTrack track in tracks) {
			track.reset();
		}
	}

	// if there is another track currently playing, the player first stops that track.
	// There will be a short pause between stopping the currently playing track and starting the new one
	public void start(int trackIndex = 0) {
        if (!isPlaying) {
			currentTrack = tracks[currentTrackIndex];
			isPlaying = true;
			nextEventTime = AudioSettings.dspTime;
			Debug.Log("BGMPlayer started");
		} else {
			Debug.Log("BGMPlayer.start() was called but BGMPlayer was already started");
		}
	}

	public void stop(bool smoothStop = true) {
        if (isPlaying) {
			isPlaying = false;
			foreach (AudioSource source in audioSources) {
				if (smoothStop) {
					fadeOut(0.06f);
				} else {
					source.Stop();
				}
			}
			Debug.Log("BGMPlayer stopped");
		} else {
			Debug.Log("BGMPlayer.stop() was called but BGMPlayer was already stopped");
		}
		reset();
	}

	public void stopSources() {
		foreach (AudioSource source in audioSources) {
			source.Stop();
		}
	}

	public void changeTrack(int index) {
		// fadeout current track
		if (isPlaying) {
			stop();
		}
		currentTrackIndex = index;
		Invoke("restartWithNewTrack", 0.5f);
	}

	public void restartWithNewTrack() {
		currentTrack = tracks[currentTrackIndex]; // currentTrackIndex is changed by changeTrack(), because Invoke can't pass any parameters to its function
		Debug.Log("queued track: " + currentTrack.name + " in BGMPlayer");
		resetVolume();
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
		Debug.Log("reset volume");
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
		stopSources();
		resetVolume();
	}
}
