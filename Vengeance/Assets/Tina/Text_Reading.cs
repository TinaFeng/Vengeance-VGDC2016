using UnityEngine;
using System.Collections;

public class Text_Reading : MonoBehaviour {

    public TextAsset textfile;
    public string[] textLines;

	// Use this for initialization
	void Start () {
	
        if(textfile != null) 
            {

            textLines = (textfile.text.Split('\n'));


            }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
