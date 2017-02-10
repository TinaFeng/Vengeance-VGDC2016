using UnityEngine;
using System.Collections;

public class Spawn_pool : MonoBehaviour {

    public float spawnTime = 5f;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Spawn () {
        GameObject obj = bullet_pool.current.GetBullet();

        if (obj == null) return;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
	}
}
