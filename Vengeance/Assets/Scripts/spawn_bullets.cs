using UnityEngine;
using System.Collections;

public class spawn_bullets : MonoBehaviour {

    public Vector3 offset1 = new Vector3(5f, 0, 0);
    public Vector3 offset2 = new Vector3(10f, 0, 0);

    public GameObject bullet1Prefab;
    public GameObject bullet2Prefab;
    public GameObject bullet3Prefab;
    
    public float delay = 5f;
    float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
	    if(timer <=0)
        {
            timer = delay;

            Instantiate(bullet1Prefab, transform.position, transform.rotation);
            Instantiate(bullet1Prefab, transform.position + offset1, transform.rotation);
            Instantiate(bullet1Prefab, transform.position - offset1, transform.rotation);
            Instantiate(bullet2Prefab, transform.position - offset2, transform.rotation);
            Instantiate(bullet3Prefab, transform.position + offset2, transform.rotation);
        }
	}
}
