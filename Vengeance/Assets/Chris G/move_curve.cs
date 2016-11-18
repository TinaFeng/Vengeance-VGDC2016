using UnityEngine;
using System.Collections;

public class move_curve : MonoBehaviour
{
    public float speed = 5f;
    public float turning_speed = 1f;
    public float angle = 180f;
    public float timer = 10f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer>0)
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
