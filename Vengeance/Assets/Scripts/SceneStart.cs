using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour {

    public GameObject boss;

	// Use this for initialization
	void Start () {
        GetComponent<ScreenFade>().fadeIn(2);
	}
	
	// Update is called once per frame
	void Update () {
        if (boss.GetComponent<EnemyStats>().health <= 0)
        {
            boss.GetComponent<EnemyStats>().health = 1;
            GetComponent<ScreenFade>().fadeOut(2);
            Invoke("MainMenu", 2);
        }
    }

    void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
