using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    public int score;
    public float speed;
    public Text livesText;
    public Text scoreText;
    public Text bombsText;
    public GameObject BombIcon; //Bomb display
    public Text AttackPatternText;
    public GameObject playerBullets;
    public GameObject bomb;
    public PoolManager poolManager;

    private bool firePressed = false;
    public int lives;
    private int bombCount;
    private Vector2 movement;
    Vector3 playerBulletOffset;
    private GameObject myObject;
    private GameObject spawnedBullet;
    private Rigidbody2D rb2d;
    private SpriteRenderer charRenderer;
    private float BombIcon_Position = 19.2f;//x-position of bombicons;
    private List<GameObject> Bombs;//A list for icon objects
    private enum attackPattern {I, II, III, IV, V};
    private float shootTime;
    attackPattern currentAttackPattern;

    // Use this for initialization
    void Start()
    {
        
        shootTime = 0;
        movement = new Vector2();
        myObject = GameObject.Find("HitDot");
        charRenderer = myObject.GetComponent<SpriteRenderer>();
        charRenderer.enabled = false;

        lives = 3;
        score = 0;
        bombCount = 3;
        Bombs = new List<GameObject>();

        updateLivesText();
        updateScoreText();
        updateBombText();
        updatePatternText();

   
        playerBulletOffset = new Vector3(0, 1, 0);

        currentAttackPattern = attackPattern.I;

    }

    //occurs every frame
    void Update()
    {
        if (Input.GetButtonDown("Bomb"))
        {
            if (bombCount != 0)
            {
                bomb.GetComponent<BulletStats>().enabled = true;
                poolManager.bombActive = true;
                bombCount -= 1;
                updateBombText();
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            speed /= 2;
            charRenderer.enabled = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            speed *= 2;
            charRenderer.enabled = false;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            playerBullets.SetActive(true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            playerBullets.SetActive(false);
        }
    }
    //occurs based on set time, independent of frame
    void FixedUpdate()
    {
        //get player's inputs
        movement.x = Input.GetAxisRaw("Horizontal") * speed/100;
        movement.y = Input.GetAxisRaw("Vertical") * speed/100;

        gameObject.transform.Translate(movement);
    }

    //function that's called when our player collides with a 2D trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("1up"))
        {
            lives++;
            updateLivesText();
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
    public void updateLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    //update our score text
    public void updateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    //update our bomb text
    void updateBombText()
    {
 
            for (int x = 0; x != Bombs.Count; x++)  //Clear the List by destroying all objexts
                Destroy(Bombs[x]);
            for (int i = 0; i <= bombCount-1; i++) //calculat the spawn distance of icons
            {
                Vector3 Iconpos = new Vector3(BombIcon_Position + (i * 1.3f), 0.3f, 0);

                GameObject clone = (GameObject)Instantiate(BombIcon, Iconpos, Quaternion.identity);
                Bombs.Add(clone);
            }

        

        bombsText.text = "Bombs: " + bombCount.ToString();
    }

    //update our pattern text
    void updatePatternText()
    {
        AttackPatternText.text = "Pattern: " + currentAttackPattern.ToString();
    }
}
