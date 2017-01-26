using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public int health=50;   //public health with the base being 50, changeable in editor
    public string movement;     // what kind of enemy this is, this will dictate how the enemy moves and which bullets the enemy will instantiate in a different script attached to all enemys
    public string bullets;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (health == 0)
        {
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
