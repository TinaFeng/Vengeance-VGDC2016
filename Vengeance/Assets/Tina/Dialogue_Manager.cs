using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue_Manager : MonoBehaviour {

    public GameObject textbox;

    public Text theText;

    public TextAsset textfile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public Player_Movement player;

    // Use this for initialization
    void Start() {

        player = FindObjectOfType<Player_Movement>();


        if (textfile != null) {

            textLines = (textfile.text.Split('\n'));

        }


    }

    // Update is called once per frame
    void Update() {

    }
}
