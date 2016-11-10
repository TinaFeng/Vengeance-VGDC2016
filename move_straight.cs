using UnityEngine;
using System.Collections;

public class move_straight : MonoBehaviour
{
    public float speed = 5f;
    public int angle = 180;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = transform.rotation;
        rot = Quaternion.Euler(0, 0, angle);
        transform.rotation = rot;

        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;
    }
}
