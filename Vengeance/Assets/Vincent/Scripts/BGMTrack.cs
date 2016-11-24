using UnityEngine;
using System.Collections;

public class BGMTrack : MonoBehaviour {

	// Types of loop algorithms available:
	//		FullRunthrough -- Play through the clip array from beginning to end, then repeat everthing
	//		AllButFirstClip -- Play through the entire array once, then repeat from the second clip (index 1)
	//		OnlyLastClip -- Play through the entire array once, then repeat only the last clip
	public enum LoopType { FullRunthrough, AllButFirstClip, OnlyLastClip }

	public BGMClip[] clips; 
	public LoopType loopType;

	private int currentClip = 0;
	private bool started = false;

	// Use this for initialization
	void Start () {
		started = false;
	}

	public BGMClip getNextClip() {
		if (!started) {			// All clip playback algorithms start from index 0 initially
			currentClip = 0;
			started = true;
			return clips[currentClip];
		} else {
			switch (loopType) {
				case LoopType.FullRunthrough:
					return FullRunthrough();
				case LoopType.AllButFirstClip:
					return AllButFirstClip();
				case LoopType.OnlyLastClip:
					return OnlyLastClip();
				default:
					Debug.LogError("No loopType selected.");
					return clips[currentClip];
			}
		}
	}

	private BGMClip FullRunthrough() {
		currentClip = (currentClip  + 1) % clips.Length;
		return clips[currentClip];
	}

	private BGMClip AllButFirstClip() {
		if (currentClip == clips.Length - 1) {
			currentClip = 1;
		} else {
			currentClip = (currentClip + 1) % clips.Length;
		}

		return clips[currentClip];
	}

	private BGMClip OnlyLastClip() {
		if (currentClip < clips.Length - 1) {
			currentClip = (currentClip + 1) % clips.Length;
		}

		return clips[currentClip];
	}

}

