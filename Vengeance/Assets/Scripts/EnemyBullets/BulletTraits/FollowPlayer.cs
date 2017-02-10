using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public string player = "Player";
    public float speed = 5f;
    public float time = 0f; //stop following player after this many seconds
    float startTime;
    // Use this for initialization
    void Start()
    {
         startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = transform.rotation;
        if (Time.time - startTime < time)
        {
            Vector3 dir = transform.position - GameObject.Find(player).transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rot = Quaternion.Euler(0, 0, angle + 90.0f);
            transform.rotation = rot;
        }
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;
    }
}