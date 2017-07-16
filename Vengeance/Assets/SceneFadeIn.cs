using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour {

    public Image image;
    float time = 0;
    public float fadeTime;

    void Start()
    {
        image.enabled = true;
    }

    // Update is called once per frame
    void Update () {
        image = GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a -= Time.deltaTime/fadeTime;
        image.color = tempColor;
        time = 0;
        if(tempColor.a < 0)
        {
            image.enabled = false;
        }
    }
}
