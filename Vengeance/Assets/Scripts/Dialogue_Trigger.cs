using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour {

    public GameObject Conversation;


    bool call;


    int health;
	void Start () {
        health = this.GetComponent<EnemyStats>().health;
        call = true;
	}
	


	void Update () {
        if (!isActiveAndEnabled)
        {
            call = true;
        }

		if(isActiveAndEnabled && call)  //if the boss showed up and we need to call it, call dialogue box by making it active
        {
            Conversation.SetActive(true);
            Conversation.GetComponent<Dialogue_Manager>().trigger = this.name+"-P";
            call = false;
        }
	}
}
