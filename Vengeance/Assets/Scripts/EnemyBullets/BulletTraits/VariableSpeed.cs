using UnityEngine;
using System.Collections;

public class VariableSpeed : MonoBehaviour
{
    public float startspeed = 20f;
    public float maxspeed = 20f;
    public float minspeed = 0;
    public float changespeed = 1f;
    public float delay = 1f;
    public float timer = 1f;

    public int angle = 180;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float timer = delay;
        Quaternion rot = transform.rotation;
        rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;

        if (startspeed > minspeed)
        {
            startspeed -= changespeed;
        }
        if (startspeed <= minspeed)
        {
            timer -= Time.deltaTime;
            if (timer<=0)
            {
                timer = delay;
                startspeed = maxspeed;
            }
        }
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, startspeed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;
    }
}
