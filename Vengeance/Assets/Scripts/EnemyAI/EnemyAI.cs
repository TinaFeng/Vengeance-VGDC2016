using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
    public int health=50;   //public health with the base being 50, changeable in editor
    public string movement;     // what kind of enemy this is, this will dictate how the enemy moves and which bullets the enemy will instantiate in a different script attached to all enemys
    public int bullets;

    public int TimeToShoot;
    private int OriginalTimeToShoot;

    public float wallDist;

	// Use this for initialization
	void Start () {
        OriginalTimeToShoot = TimeToShoot;

    }

    // Update is called once per frame
    void FixedUpdate() {

        if (movement == "MoveToPoint")
        {
            gameObject.GetComponent<EnemyMovement>().MoveToPoint();
        }
        if (movement.Substring(0, 6) == "Bounce")
        {
            gameObject.GetComponent<EnemyMovement>().Bounce(wallDist, movement.Substring(6));
        }

        if (health <= 0)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().score += 100;
            Destroy(gameObject);
            
        }

        TimeToShoot--;
        if (TimeToShoot < 0)
        {
            //gameObject.GetComponent<BulletFactory>().BulletMaker(bullets);
            TimeToShoot = OriginalTimeToShoot;
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
