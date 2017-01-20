using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public GameObject dialogueUI;
    public DialogueHandler dialogueScript;

    void Start()
    {
        dialogueScript = GameObject.FindGameObjectWithTag("DialogueUI").GetComponent<DialogueHandler>();
        dialogueUI = dialogueScript.dialogueUI;
        
        dialogueUI.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateDialogue(0);
        }
    }

    public void ActivateDialogue(int index)
    {
        dialogueUI.SetActive(true);
        dialogueScript.GetComponent<DialogueHandler>().LoadTextAsset(index);
        dialogueScript.enabled = true;
        dialogueScript.SetBackground("");
    }

}
