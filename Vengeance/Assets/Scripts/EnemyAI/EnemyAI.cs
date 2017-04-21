using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public int health=50;   //public health with the base being 50, changeable in editor

    // Update is called once per frame
    void FixedUpdate() {
        if (health <= 0)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().score += 100;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "playerBullet")
        {
            health -= 10;
        }
    }
}
