using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tracker : MonoBehaviour {

    public GameObject Boss;
    private float Boss_Position;
    private bool active = false;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(active != Boss.activeSelf)
        {
            GetComponent<SpriteRenderer>().enabled = Boss.activeSelf;
            active = Boss.activeSelf;
        }
        if (active)
        {
            Boss_Position = Boss.GetComponent<Rigidbody2D>().position.x;
            this.transform.position = new Vector3(Boss_Position, this.transform.position.y, this.transform.position.z);    
        }
    }
}
