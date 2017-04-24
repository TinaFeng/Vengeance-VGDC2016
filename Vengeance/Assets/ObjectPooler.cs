using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public GameObject objectPooled;
    public int pooledAmount;
    public Stack<GameObject> deactivatedObjects;
    

	// Use this for initialization
	void Start () {
        deactivatedObjects = new Stack<GameObject>();

		for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectPooled, transform);
            obj.GetComponent<BulletStats>().objectPool = this;
        }
	}

    public GameObject GetObject()
    {
        if(deactivatedObjects.Count > 0)
        {
            return deactivatedObjects.Pop();
        }
        else
        {
            GameObject obj = (GameObject)Instantiate(objectPooled, transform);
            obj.GetComponent<BulletStats>().objectPool = this;
            return obj;
        }
    }
}
