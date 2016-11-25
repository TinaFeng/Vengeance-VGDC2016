using UnityEngine;
using System.Collections;

public class playerBulletMove : MonoBehaviour {

    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start ()
    {
        //get our rigidbody
        rb2d = GetComponent<Rigidbody2D>();

        //temporary way to get rid of it
        Destroy(gameObject, 2);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        rb2d.velocity = Vector3.up * 10f;
    }
}
