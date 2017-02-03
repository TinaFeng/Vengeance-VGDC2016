using UnityEngine;
using System.Collections;

public class SpawnShotgun : MonoBehaviour {

    public GameObject bullet;
    public float delay = 5f;
    float timer = 0;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = delay;
            Vector3 dir = transform.position - GameObject.Find("Player").transform.position;
            float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90.0f;
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle-10.0f));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle+10.0f));
        }

    }
}
