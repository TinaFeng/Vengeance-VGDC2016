using UnityEngine;
using System.Collections;

public class SpawnPerpendicularBullet : MonoBehaviour {

    public GameObject bullet;
    public float delay = 0.5f;
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
            Debug.Log(Mathf.Sin(gameObject.GetComponent<Rigidbody2D>().velocity.x + gameObject.GetComponent<Rigidbody2D>().velocity.y));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 90));
            Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 270));
        }
    }
}
