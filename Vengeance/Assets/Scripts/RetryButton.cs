using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RetryButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        //restart the game simulation
        Time.timeScale = 1;
    }
}
