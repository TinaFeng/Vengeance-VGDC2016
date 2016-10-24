using UnityEngine;
using System.Collections;

public class BGMPlayer : MonoBehaviour {

	public AudioSource audioSource;	
	public AudioClip[] clipList;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
