using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    int hiScore;
    public int score;
    public float speed;
    public Text hiScoreText;
    public Text scoreText;
    public GameObject BombIcon; //Bomb display
    public GameObject LifeIcon;
    public GameObject playerBullets;
    public GameObject bomb;
    public PoolManager poolManager;
    public bool iFrames; //If true, player invincible
    public float iFrameTimer; //How long we want the player to be invincible on damage
    public GameObject gameOverScreen;

    private bool firePressed = false;
    public int lives;
    private int bombCount;
    private Vector2 movement;
    Vector3 playerBulletOffset;
    private GameObject myObject;
    private GameObject spawnedBullet;
    private Rigidbody2D rb2d;
    private SpriteRenderer charRenderer;
    private GameObject Bomb_UI;
    private GameObject Life_UI;
    private float BombIcon_Position;//x-position of bombicons;
    private float LifeIcon_Position;//x-position of bombicons;
    private List<GameObject> Bombs;//A list for icon objects
    private List<GameObject> Lives;//A list for icon objects
    private float shootTime;
    private bool iFrameBlink, isGamePaused, isGameOver; //iFrame, game pause, and game over flags

    // Use this for initialization
    void Start()
    {
        if(!PlayerPrefs.HasKey("Hi-Score"))
        {
            PlayerPrefs.SetInt("Hi-Score", 0);
        }
        else
        {
            hiScore = PlayerPrefs.GetInt("Hi-Score");
        }
        shootTime = 0;
        movement = new Vector2();
        myObject = GameObject.Find("HitDot");
        charRenderer = myObject.GetComponent<SpriteRenderer>();
        charRenderer.enabled = false;

        lives = 3;
        score = 0;
        bombCount = 3;
        Bombs = new List<GameObject>();
        Lives = new List<GameObject>();


        Bomb_UI = GameObject.FindGameObjectWithTag("BombIcons");
        Life_UI = GameObject.FindGameObjectWithTag("PlayerIcons");
        Debug.Log(Life_UI.name);
        BombIcon_Position = BombIcon.transform.position.x;
        LifeIcon_Position = LifeIcon.transform.position.x;
        updateLivesText();
        updateHiScoreText();
        updateScoreText();
        updateBombText();

        //player does not start invincible, sorry :P
        iFrames = iFrameBlink = false; //default to not blinking and not invulnerable
        iFrameTimer = 2.0f; // default 1 sec invulnerability

        isGameOver = isGamePaused = false; //game is not over or paused by default

        gameOverScreen.SetActive(false);
    }

    //occurs every frame
    void Update()
    {
        if (!isGameOver && !isGamePaused) //only update the game logic if the game isn't over or isn't paused
        {
            if(!iFrames)
                score++;
            updateScoreText();

            if (Input.GetButtonDown("Bomb"))
            {
                if (bombCount != 0)
                {
                    bomb.GetComponent<BulletStats>().enabled = true;
                    bomb.GetComponent<SpriteRenderer>().enabled = true;
                    poolManager.bombActive = true;
                    bombCount -= 1;
                    updateBombText();
                }
            }
            if (Input.GetButtonDown("Fire2"))
            {
                //poolManager.timeStop = true;
                speed /= 2;
                charRenderer.enabled = true;
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                //poolManager.timeStop = false;
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
        } //code below this line will still run when the game is paused or over

       
    }

    //occurs based on set time, independent of frame
    void FixedUpdate()
    {
        if (!isGameOver && !isGamePaused) //only update the game logic if the game isn't over or isn't paused
        {
            //get player's inputs
            movement.x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
            movement.y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

            gameObject.transform.Translate(movement);

            if (iFrames) //if we go invincible
            {
                if (iFrameTimer > 0)
                {
                    //reduce time
                    iFrameTimer -= Time.deltaTime;

                    if (!iFrameBlink) //if player is visible
                    {
                        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255); //make the player transparent
                        iFrameBlink = !iFrameBlink; // toggle iframe blink
                    }
                    else //player is blinking
                    {
                        GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255); //make the player apparent
                        iFrameBlink = !iFrameBlink; // toggle iframe blink
                    }
                }
                else //turn off iframes, reset timer, clear blink flag
                {
                    iFrames = false; //turn off iframes
                    iFrameTimer = 2.0f; //reset timer
                    if (iFrameBlink) GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255); //Ensure the player is visible if they weren't
                    iFrameBlink = false; //ensure iframe isn't blinking anymore
                }
            }
        } //anything below this will still run when the game is paused or over


    }

    //function that's called when our player collides with a 2D trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("1up"))
        {
            lives++;
            updateLivesText();
        }
    }
    //update our lives text
    public void updateLivesText()
    {
        for (int x = 0; x != Lives.Count; x++)  //Clear the List by destroying all objexts
            Destroy(Lives[x]);
        for (int i = 0; i <= lives - 1; i++) //calculat the spawn distance of icons
        {
            Vector3 Iconpos = new Vector3(LifeIcon_Position + (i * 20f) - 70f, LifeIcon.transform.position.y - 20, 0);
            GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
            GameObject clone = (GameObject)Instantiate(LifeIcon, Iconpos, Quaternion.identity);
            clone.transform.SetParent(Life_UI.transform, false);
            clone.layer = 5;
            Lives.Add(clone);
        }
    }

    //update our score text
    public void updateScoreText()
    {
        scoreText.text = score.ToString();
        if(PlayerPrefs.GetInt("Hi-Score") < score)
        {
            hiScore = score;
            PlayerPrefs.SetInt("Hi-Score", hiScore);
            updateHiScoreText();
        }
    }

    public void updateHiScoreText()
    {
        hiScoreText.text = hiScore.ToString();
    }

    //update our bomb text
    void updateBombText()
    {
        for (int x = 0; x != Bombs.Count; x++)  //Clear the List by destroying all objexts
            Destroy(Bombs[x]);
        for (int i = 0; i <= bombCount-1; i++) //calculat the spawn distance of icons
        {
            Vector3 Iconpos = new Vector3(BombIcon_Position + (i * 20f) - 70f, BombIcon.transform.position.y -20, 0);
            GameObject Canvas = GameObject.FindGameObjectWithTag("Canvas");
            GameObject clone = (GameObject)Instantiate(BombIcon, Iconpos, Quaternion.identity);
            clone.transform.SetParent(Bomb_UI.transform, false);
            clone.layer = 5;
            Bombs.Add(clone); 
        }
    }

    //when our player is damaged, cause their sprite to flash for a few moments, and give them invincibility for that duration
    public void playerDamaged()
    {
        bomb.GetComponent<BulletStats>().enabled = true;
        bomb.GetComponent<SpriteRenderer>().enabled = true;
        poolManager.bombActive = true;
        iFrames = true; // set us invincible, which kicks off the blink code
    }

    //called when the game is over
    public void gameOver()
    {
        //set game over
        isGameOver = true;
        //disable the player
        GetComponent<SpriteRenderer>().enabled = false;
        //stop the game simulation
        Time.timeScale = 0;
        //turn on the gameover menu
        gameOverScreen.SetActive(true);
    }
}
