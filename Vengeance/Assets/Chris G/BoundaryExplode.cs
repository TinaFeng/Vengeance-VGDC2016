﻿using UnityEngine;
using System.Collections;

public class BoundaryExplode : MonoBehaviour
{
    public GameObject bullet;
    public float direction = 0;
    float BoundaryRadius = 0.0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (pos.y + BoundaryRadius >= Camera.main.orthographicSize)
        {
            
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 180));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 135));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 225));
            Destroy(this);
            
        }
        if (pos.y - BoundaryRadius <= -Camera.main.orthographicSize)
        {
            
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 0));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 45));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 315));
            Destroy(this);
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float widthOrtho = Camera.main.orthographicSize * screenRatio;

        if (pos.x + BoundaryRadius >= widthOrtho)
        {
            
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 90));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 45));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 135));
            Destroy(this);
        }
        if (pos.x - BoundaryRadius <= -widthOrtho)
        {
            
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 270));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 315));
            Instantiate(bullet, transform.position, Quaternion.Euler(direction, 0, 225));
            Destroy(this);
        }
    }
}
