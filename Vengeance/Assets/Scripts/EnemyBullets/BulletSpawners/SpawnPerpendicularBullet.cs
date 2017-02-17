using UnityEngine;
using System.Collections;

public class SpawnPerpendicularBullet : MonoBehaviour {

    public GameObject bullet;
    public float delay;
    float timer;

    // Use this for initialization
    void Start () {
        timer = delay;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = delay;
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, (gameObject.transform.rotation.eulerAngles.z) - 90));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, (gameObject.transform.rotation.eulerAngles.z) + 90));
        }
    }
}
