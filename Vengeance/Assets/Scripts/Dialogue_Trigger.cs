using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour {

    public GameObject Conversation;
    // Use this for initialization

    bool call = true;
	void Start () {
//        Conversation = GameObject.FindGameObjectWithTag("Dialogue_Box");

	}
	
	// Update is called once per frame
	void Update () {
		if(isActiveAndEnabled &&call)
        {
            Conversation.SetActive(true);
            call = false;
        }
	}
}
