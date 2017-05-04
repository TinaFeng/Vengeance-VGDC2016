using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    GameObject player;
    public int health = 50;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.SetActive(false);
        health -= 10;
        if (health <= 0)
        {
            player.GetComponent<PlayerController>().score += 100;
            gameObject.SetActive(false);
        }
    }
}
