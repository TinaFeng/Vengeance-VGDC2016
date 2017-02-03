using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bullet_pool : MonoBehaviour {

    public static bullet_pool current;
    public GameObject bullet;
    public int pooledAmount = 10;
    public bool willGrow = true;

    List<GameObject> bullets;

    void Awake()
    {
        current = this;
    }

	// Use this for initialization
	void Start () {
        bullets = new List<GameObject>();
        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            obj.SetActive(false);
            bullets.Add(obj);
        }
	
	}
	
	// Update is called once per frame
	public GameObject GetBullet()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            return bullets[i];

        }

        if(willGrow)
        {
            GameObject obj = (GameObject)Instantiate(bullet);
            bullets.Add(obj);
            return obj;
        }
        return null;
    }
}
