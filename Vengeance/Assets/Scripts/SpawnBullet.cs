using UnityEngine;
using System.Collections;

public class SpawnBullet : MonoBehaviour
{
    public GameObject bullet;

    public float delay = 5f;
    float timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = delay;

            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
