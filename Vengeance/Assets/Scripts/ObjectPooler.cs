﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public GameObject objectPooled;
    public int pooledAmount;
    public Stack<GameObject> deactivatedObjects;
    public bool extraPool;
    public ObjectPooler otherPool;

    

	// Use this for initialization
	void Start () {
        deactivatedObjects = new Stack<GameObject>();

		for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectPooled, transform);
            obj.GetComponent<BulletStats>().objectPool = this;
            if (extraPool)
            {
                obj.GetComponent<SpawnBullet>().bulletPool = otherPool;
            }
            obj.SetActive(false);
        }
	}

    void FixedUpdate()
    {
        //Debug.Log(pooledAmount - deactivatedObjects.Count);
    }

    public GameObject GetObject()
    {
        if(deactivatedObjects != null && deactivatedObjects.Count > 0)
        {
            return deactivatedObjects.Pop();
        }
        else
        {
            GameObject obj = (GameObject)Instantiate(objectPooled, transform);
            obj.GetComponent<BulletStats>().objectPool = this;
            if (extraPool)
            {
                obj.GetComponent<SpawnBullet>().bulletPool = otherPool;
            }
            pooledAmount++;
            return obj;
        }
    }
}
