using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public AudioSource audioSource;	
	public AudioClip[] clipList;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = clipList[0];
		audioSource.loop = true;
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
