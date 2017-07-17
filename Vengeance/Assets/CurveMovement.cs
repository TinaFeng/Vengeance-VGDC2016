using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveMovement : MonoBehaviour {

    PoolManager poolManager;
    public AnimationCurve locX;
    public AnimationCurve locY;
    public float timeToKill = 0;
    float time = 0;
    Vector3 prev;
    Vector3 temp;

    private Animator anim;

    void Start()
    {
        if (poolManager == null)
        {
            poolManager = GameObject.Find("ObjectPooling").GetComponent<PoolManager>();
        }
        prev.Set(locX.Evaluate(time), locY.Evaluate(time), 0f);
        anim = GetComponent<Animator>();
    }
	void FixedUpdate () {
        if (!poolManager.timeStop)
        {
            if (poolManager.timeSlow)
                time += Time.deltaTime * poolManager.timeSlowAmount;
            else
                time += Time.deltaTime;
        }
        
        temp.Set(locX.Evaluate(time), locY.Evaluate(time), 0f);
        transform.position = temp;
        /////Determine direction between updates
        if (gameObject.tag == "Boss")
        {
            if (temp.x - prev.x > 0)
                anim.SetBool("Right", true);


            else if (temp.x - prev.x < 0)
                anim.SetBool("Left", true);

            else
            {
                anim.SetBool("Left", false);
                anim.SetBool("Right", false);
            }

            prev = temp;
        }
        if (time > timeToKill && timeToKill != 0)
        {
            gameObject.SetActive(false);
        }
    }
}