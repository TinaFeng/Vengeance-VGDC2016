using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Movement : MonoBehaviour
{
    public float speed = 5f;
    public float changeangle = 10f;
    public float delay = .25f;
    public float timer = .25f;

    int x = 0;
    int turn = 0;
    float angle = 0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (x == 0)
        {
            angle = transform.rotation.z-changeangle;
            x = 1;
        }
        timer -= Time.deltaTime;
        if (turn == 0)
        {
            angle += changeangle;
        }
        else
        {
            angle -= changeangle;
        }
        if (timer <= 0)
        {
            timer = delay;
            if (turn == 0)
            {
                turn = 1;
            }
            else
            {
                turn = 0;
            }
        }
        Quaternion rot = transform.rotation;
        rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;

    }
}
