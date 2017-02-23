using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

    //the Rigidbody2D of our bullet
    private Rigidbody2D rb2d;
    public float speed = 20f;

    // Use this for initialization
    void Start ()
    {
        //get our rigidbody
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	//occurs on a set timer independent of frame
	void FixedUpdate ()
    {
        //set velocity to move bullet up by a vector of 10 / update
        rb2d.velocity = Vector3.up * speed;
    }
}
