using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    //public variables
    public float speed;

    //private variables
    private Vector2 movement;
    private Vector2 halfPlayerDimensions;
    private Vector2 cameraDimensions;

    private Rigidbody2D rb2d;

    private SpriteRenderer charRenderer;

    private GameObject myObject;

    //z = fire; x = bomb; c = cycle through abilities
    public enum myAttackKeys {FIRE = 'Z', BOMB = 'X', CYCLE = 'C'};

	// Use this for initialization
	void Start () {
        //initialize movement to a vector2(x,y)
        movement = new Vector2();

        //pass the Rigidbody2D object associated with our player into rb2d memory space
        rb2d = GetComponent<Rigidbody2D>();

        //pass the HitDot (child of player) game object to the myObject memory space
        myObject = GameObject.Find("HitDot");

        //pass the SpriteRenderer component of the HitDot child object, and pass it to the charRenderer memory space
        charRenderer = myObject.GetComponent<SpriteRenderer>();
        //disable the dot so that it only shows up when keys are depressed
        charRenderer.enabled = false;


        //capture half the player's width and height for calculating collsion with camera frustum
        //Sprite tempSprite = charRenderer.sprite;
        Vector4 tempBorder = charRenderer.sprite.border;
        //print("X: " + tempBorder.x + ", Y: " + tempBorder.y);

        GameObject tempCamera = GameObject.Find("Main Camera");

        //capture camera's width and height
        //cameraDimensions.x
        //cameraDimensions.y = Camera.current.rect.height;

        print("X: " + cameraDimensions.x + ", Y: " + cameraDimensions.y);
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

        //get player's inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //apply them to the character
        rb2d.velocity = movement * speed;

        //check to see if the player is out of bounds, and if so, keep them in bounds

	}
}
