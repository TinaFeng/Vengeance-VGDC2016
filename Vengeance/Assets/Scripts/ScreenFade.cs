using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

    public Image image;
    float t;
    float time;
    bool fade = true;

    // Update is called once per frame
    void Update () {
        if (fade)
        {
            t += Time.deltaTime / time;
            image = GetComponent<Image>();
            var tempColor = image.color;
            tempColor.a = Mathf.Lerp(0, 1, t);
            image.color = tempColor;
            if (t < 0 || t > 1)
                fade = false;
        }
    }

    public void fadeIn(float x)
    {
        time = -x;
        t = 1;
        image.enabled = true;
        fade = true;
    }

    public void fadeOut(float x)
    {
        time = x;
        t = 0;
        image.enabled = true;
        fade = true;
    }
}
