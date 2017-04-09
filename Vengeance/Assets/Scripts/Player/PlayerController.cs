using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    /**Public Variables**/

    //Floats
    public float speed;

    //Texts
    public Text livesText;
    public Text scoreText;
    public Text bombsText;
    public Text AttackPatternText;

    //Game Objects
    public GameObject playerBullet;

    /**private variables**/

    //flags
    private bool firePressed = false;

    //Ints
    private int lives;
    public int score;
    private int bombCount;

    //Vectors
    private Vector2 movement;
    Vector3 playerBulletOffset;

    //Objects & Components
    private GameObject myObject;
    private GameObject spawnedBullet;

    private Rigidbody2D rb2d;

    private SpriteRenderer charRenderer;

    //z = fire; x = bomb; c = cycle through abilities
    //1-5 bullets per shot
    private enum attackPattern {I, II, III, IV, V};
    private float shootTime;
    //Enumerated Variables
    attackPattern currentAttackPattern;

    // Use this for initialization
    void Start()
    {
        shootTime = 0;
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
        bombCount = 2;

        //Initialize our textboxes
        updateLivesText();
        updateScoreText();
        updateBombText();
        updatePatternText();

        //initialize our bullet offset
        playerBulletOffset = new Vector3(0, 1, 0);

        //Set current attack pattern to I
        currentAttackPattern = attackPattern.I;
    }

    //occurs every frame
    void Update()
    {
        
        //handle graze mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 5;
            charRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10;
            charRenderer.enabled = false;
        }

        //handle spawning bullets
        if (Input.GetKeyDown(KeyCode.Z)) { firePressed = true; }
        else if(Input.GetKeyUp(KeyCode.Z)) { firePressed = false; }

        shootTime += Time.deltaTime;
        if (shootTime > 0.07 && firePressed)
        {
            playerShoot();
            shootTime = 0;
        }
    }

    //occurs based on set time, independent of frame
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
        else if (other.gameObject.CompareTag("Enemy"))
        {
            lives--;
            updateLivesText();

            if (currentAttackPattern > attackPattern.I)
            {
                currentAttackPattern--;
                updatePatternText();
            }
        }
        /*else if (other.gameObject.CompareTag("pBlock"))
        {
            score += 100;
            updateScoreText();
        }*/
        else if (other.gameObject.CompareTag("pBlock"))
        {
            //level up our attack pattern, otherwise give us a bomb
            if (currentAttackPattern == attackPattern.V)
            {
                bombCount++;
                updateBombText();
            }
            else
            {
                currentAttackPattern++;
                updatePatternText();
            }

            //increase our score
            score += 1000;
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

    //update our bomb text
    void updateBombText()
    {
        bombsText.text = "Bombs: " + bombCount.ToString();
    }

    //update our pattern text
    void updatePatternText()
    {
        AttackPatternText.text = "Pattern: " + currentAttackPattern.ToString();
    }

    //playershoot
    void playerShoot()
    {
        //Instantiate the player's bullets at our current location, slightly offset depending on the current pattern
        switch (currentAttackPattern)
        {
            case attackPattern.V:
                spawnBullet(Vector3.up +Vector3.left);
                spawnBullet(Vector3.up * 1.1f + Vector3.left * 0.5f);
                spawnBullet(Vector3.up * 1.2f);
                spawnBullet(Vector3.up * 1.1f + Vector3.right * 0.5f);
                spawnBullet(Vector3.up + Vector3.right);
                break;
            case attackPattern.IV:
                spawnBullet(Vector3.up + Vector3.left);
                spawnBullet(Vector3.up + Vector3.left * 0.5f);
                spawnBullet(Vector3.up + Vector3.right * 0.5f);
                spawnBullet(Vector3.up + Vector3.right);
                break;
            case attackPattern.III:
                spawnBullet(Vector3.up + Vector3.left);
                spawnBullet(Vector3.up * 1.1f);
                spawnBullet(Vector3.up + Vector3.right);
                break;
            case attackPattern.II:
                spawnBullet(Vector3.up + Vector3.left);
                spawnBullet(Vector3.up + Vector3.right);
                break;
            default: //This is implicitly case I
                spawnBullet(Vector3.up);
                break;
        }

    }

    //spawn a bullet at the player's location and adjusted by offset
    void spawnBullet(Vector3 offset)
    {
        spawnedBullet = (GameObject)Instantiate(playerBullet, transform.position + offset, transform.rotation);
    }
}
