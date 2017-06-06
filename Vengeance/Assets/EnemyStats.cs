using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    GameObject player;
	ParticleSystem deatheffect;
    public int health = 50;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		deatheffect = GetComponent<ParticleSystem> ();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.SetActive(false);
        health -= 10;


        if (health <= 0)
		{
			StartCoroutine(PaticleTime ());
            player.GetComponent<PlayerController>().score += 100;

            
        }
    }

	IEnumerator PaticleTime()

	{	
		if (gameObject.tag == "Boss") {
			
			deatheffect.Play ();
			deatheffect.Play ();
			deatheffect.Play ();
			this.GetComponent<SpriteRenderer> ().enabled = false;
			yield return new WaitForSeconds (2f);
		} 
		else {
			this.GetComponent<SpriteRenderer> ().enabled = false;
			deatheffect.Play ();
		
			yield return new WaitForSeconds (0.2f);

		}


		gameObject.SetActive(false);
	}
}
