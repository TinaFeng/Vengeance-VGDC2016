using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_Switch : MonoBehaviour {


    public GameObject ImageOnPanel;  ///set this in the inspector
    private RawImage img;

    void Start()
    {
        img = (RawImage)ImageOnPanel.GetComponent<RawImage>();

        img.texture = (Texture)Resources.Load("Alicia");

    }


    // Update is called once per frame
    void Update()
    {

    }
}