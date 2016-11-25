using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    /**Public Variables**/

    //Floats
    public float speed;

    //Texts
    public Text livesText;
    public Text scoreText;


    /**private variables**/

    //Ints
    private int lives;
    private int score;

    //Vectors
    private Vector2 movement;

    //Objects & Components
    private GameObject myObject;

    private Rigidbody2D rb2d;

    private SpriteRenderer charRenderer;

    //z = fire; x = bomb; c = cycle through abilities
    public enum myAttackKeys { FIRE = 'Z', BOMB = 'X', CYCLE = 'C' };

    // Use this for initialization
    void Start()
    {
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

        //initialize our score and lives
        lives = 3;
        score = 0;

        //Initialize our textboxes
        updateLivesText();
        updateScoreText();
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
    void FixedUpdate()
    {
        //get player's inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //apply them to the character
        rb2d.velocity = movement * speed;
    }

    //function that's called when our player collides with a 2D trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("1up"))
        {
            lives++;
            updateLivesText();
        }
        else if (other.gameObject.CompareTag("bullet"))
        {
            lives--;
            updateLivesText();
        }
        else if (other.gameObject.CompareTag("pBlock"))
        {
            score += 100;
            updateScoreText();
        }

    }
    //update our lives text
    void updateLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    //update our score text
    void updateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

}
