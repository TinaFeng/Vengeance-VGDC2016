using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public float speed;
    private Vector2 movement;
    private Rigidbody2D rb2d;
    private SpriteRenderer charRenderer;
    private GameObject myObject;

	// Use this for initialization
	void Start () {
        movement = new Vector2();
        rb2d = GetComponent<Rigidbody2D>();
        myObject = GameObject.Find("HitDot");
        charRenderer = myObject.GetComponent<SpriteRenderer>();
        charRenderer.enabled = false;
	}
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 3;
            charRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10;
            charRenderer.enabled = false;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb2d.velocity = movement * speed;

	}
}
