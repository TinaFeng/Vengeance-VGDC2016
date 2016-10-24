using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public AudioSource audioSource;	
	public BGMTrack track;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = track.getNextClip(true);
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
