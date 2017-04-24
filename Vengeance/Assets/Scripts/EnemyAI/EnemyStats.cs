using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    GameObject player;
    public int startHealth = 50;
    int health;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = startHealth;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "playerBullet")
        {
            health -= 10;
            if (health <= 0)
            {
                player.GetComponent<PlayerController>().score += 100;
                gameObject.SetActive(false);
            }
        }
    }
}
