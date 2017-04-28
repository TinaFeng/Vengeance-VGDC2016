using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveMovement : MonoBehaviour {

    public AnimationCurve locX;
    public AnimationCurve locY;
    public float timeToKill = 0;
    float time = 0;
    Vector3 temp;
        
	void FixedUpdate () {
        time += Time.deltaTime;
        temp.Set(locX.Evaluate(time), locY.Evaluate(time), 0f);
        transform.position = temp;
        if(time > timeToKill && timeToKill != 0)
        {
            gameObject.SetActive(false);
        }
    }
}