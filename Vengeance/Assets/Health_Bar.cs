using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health_Bar : MonoBehaviour {

    public float initialHealth;
    public float health;
    public  GameObject healthbar;
    public Material health_shader;
    public float lol;

    // Use this for initialization
    void Start()
    {
        initialHealth = gameObject.GetComponent<EnemyStats>().health;
        healthbar = GameObject.Find("HealthBarImage");
        health_shader = healthbar.GetComponent<Image>().material;
        health = initialHealth;
    }
	
	// Update is called once per frame
	void Update () {

        lol = health / initialHealth;
        health = gameObject.GetComponent<EnemyStats>().health;

        health_shader.SetFloat("_range",Mathf.Abs( 1-(health / initialHealth)));

    }
}
