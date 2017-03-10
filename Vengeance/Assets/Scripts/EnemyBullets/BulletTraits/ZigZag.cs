using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZag : MonoBehaviour {
    public float speed = 5f;
    public float changespeed = 1f;
    public float delay = .25f;
    public float timer = .25f;
    int x = 0;
    int turn = 0;
    float angle = 0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (x == 0)
        {
            angle = transform.rotation.z - 45f;
            x = 1;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = delay;
            if (turn == 0)
            {
                turn = 1;
                angle += 90f;
            }
            else
            {
                turn = 0;
                angle -= 90f;
            }
        }
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;

    }
}
