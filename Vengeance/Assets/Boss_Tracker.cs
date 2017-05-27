using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tracker : MonoBehaviour {

    private GameObject Boss;
    private float Boss_Position;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
 
    }
	
	// Update is called once per frame
	void Update () {

        Boss = GameObject.FindGameObjectWithTag("Boss");
        
        if (Boss == null)
        {
            GetComponent<SpriteRenderer>().enabled = false;

        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;

            Boss_Position = Boss.GetComponent<Rigidbody2D>().position.x;
            this.transform.position = new Vector3(Boss_Position, this.transform.position.y, this.transform.position.z);
           
        }
    }
}
