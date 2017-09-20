using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    PoolManager poolManager;
    GameObject player;
	ParticleSystem deatheffect;
    public int health = 50;
    public int score;
    public bool boss;
    public BulletStats bossBomb;

    void Start()
    {
        if (poolManager == null)
        {
            poolManager = GameObject.Find("ObjectPooling").GetComponent<PoolManager>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
		deatheffect = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.SetActive(false);
        health -= 10;
        if (health <= 0)
		{
            if (boss)
            {
                poolManager.bossBombActive = true;
                bossBomb.transform.position = this.transform.position;
                bossBomb.GetComponent<SpriteRenderer>().enabled = true;
                bossBomb.enabled = true;
            }
            gameObject.SetActive(false);
            player.GetComponent<PlayerController>().score += score;
        }
    }
}
