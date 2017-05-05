﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public int score;
    public float speed;
    public Text livesText;
    public Text scoreText;
    public Text bombsText;
    public Text AttackPatternText;
    public ObjectPooler BulletPool;

    private bool firePressed = false;
    private int lives;
    private int bombCount;
    private Vector2 movement;
    Vector3 playerBulletOffset;
    private GameObject myObject;
    private GameObject spawnedBullet;
    private Rigidbody2D rb2d;
    private SpriteRenderer charRenderer;
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
        bombCount = 2;

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
        
        //handle graze mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed/=2;
            charRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed*=2;
            charRenderer.enabled = false;
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
}