using UnityEngine;
using System.Collections;

public class MoveTowardPlayer : MonoBehaviour
{

    public float speed = 5f;
    public string player = "PlayerShip";

    int x = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = transform.rotation;
        if (x == 0)
        {
            Vector3 dir = transform.position - GameObject.Find(player).transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rot = Quaternion.Euler(0, 0, angle + 90.0f);
            transform.rotation = rot;
        }
        x = 1;
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;
    }
}
