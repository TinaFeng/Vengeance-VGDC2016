using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

    public float speed = 20f;

	void FixedUpdate ()
    {
        transform.Translate(0, speed*Time.deltaTime, 0);
    }
}
