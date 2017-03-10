﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCurve : MonoBehaviour
{
    public float speed = 5f;
    public float turning_speed = 1f;
    public float timer = 10f;
    int x = 0;
    float angle = 0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (x == 0)
        {
            angle = transform.rotation.z - 45f;
            x = 1;
        }
        if (timer > 0)
        {
            angle += turning_speed;
        }
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;
    }
}
