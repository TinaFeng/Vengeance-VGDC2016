using UnityEngine;
using System.Collections;

public class TEMP_GameManager : MonoBehaviour {

	public BGMPlayer bgmPlayer;

	private static bool DEBUG_fadeCoroutineCalled = false;
	// Use this for initialization
	void Start () {
		bgmPlayer = GetComponentInChildren<BGMPlayer>();
	}

	// Update is called once per frame
	void Update () {
		if (!DEBUG_fadeCoroutineCalled) {
			StartCoroutine(DEBUG_fadeTestCoroutine());
			DEBUG_fadeCoroutineCalled = true;
		}
	}

	private IEnumerator DEBUG_fadeTestCoroutine() {
		DEBUG_waitThenFadeOutMusic();
		
		// fadeOut is also a coroutine, so this WaitForSeconds needs to be longer than the fadeOut
		// just so that fadeIn isn't called while fadeOut is still executing
		yield return new WaitForSeconds(4.0F);
		
		DEBUG_waitThenFadeInMusic();			
	}		
		
	void DEBUG_waitThenFadeInMusic() {
		StartCoroutine(DEBUG_waitFadeIn());
	}

	void DEBUG_waitThenFadeOutMusic() {
		StartCoroutine(DEBUG_waitFadeOut());
	}

	private IEnumerator DEBUG_waitFadeIn() {
		yield return new WaitForSeconds(3.0F);
		bgmPlayer.fadeIn(lengthInSeconds: 3.0F, interval: 0.05F);
	}

	private IEnumerator DEBUG_waitFadeOut() {
		yield return new WaitForSeconds(3.0F);
		bgmPlayer.fadeOut(lengthInSeconds: 3.0F, interval: 0.05F);
	}
}