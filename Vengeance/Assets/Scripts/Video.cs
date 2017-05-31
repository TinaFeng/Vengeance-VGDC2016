using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class Video : MonoBehaviour {


    VideoPlayer vp;
	// Use this for initialization
	void Start () {

        vp = GetComponent<VideoPlayer>();
        StartCoroutine(wait(vp.clip.length));

    }

    // Update is called once per frame
    void Update () {
		
	}


    IEnumerator wait(double time)
    {

        float timef = (float)(time-1.5f);
        Debug.Log(timef);
        yield return new WaitForSeconds(timef);
        Destroy(vp);
    }
}
