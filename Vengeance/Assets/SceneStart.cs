using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<ScreenFade>().fadeIn(2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
