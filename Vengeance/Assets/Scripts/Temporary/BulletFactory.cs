using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour {

    //Factory to instatiate bullets
    // bullet is chosen based on a list index in the enemy class 



    public List<GameObject> bullets = new List<GameObject>();


    public GameObject BulletMaker(int index)
    {
        return Instantiate(bullets[index]);
    }

}
