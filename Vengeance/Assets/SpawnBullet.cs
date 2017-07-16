using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position {Forward = 1, Radial = 2, Chaos = 4};
public enum Frequency {Repeating = 1, Disable = 2};

public class SpawnBullet : MonoBehaviour {

    public ObjectPooler bulletPool;
    public Position posType;
    public Frequency freqType;
    string functionName;
    GameObject obj;
    GameObject target;

    public Vector3 offset;
    public AnimationCurve dir;
    public bool towardPlayer;
    Vector3 tPlayer;
    float angle;
    float deltaTime;

    //Radial
    public float radMin;
    public float radMax;
    public int radNum;

    //Chaos
    public float chaMin;
    public float chaMax;

    //Repeating
    public float repFreq;
    public float repDelay;
    public int shotMax;
    int shotCount = 0;

	void OnEnable () {
        deltaTime = 0;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        if (((int)posType & 1) == 1)
        {
            functionName = "Forward";
        }
        if (((int)posType & 2) == 2)
        {
            functionName = "Radial";
        }
        if (((int)posType & 4) == 4)
        {
            functionName = "Chaos";
        }
        if (((int)freqType & 2) != 2)
        {
            InvokeRepeating(functionName, repDelay, repFreq);
        }
    }
	
    void FixedUpdate()
    {
        deltaTime += Time.deltaTime;
    }
	void Forward()
    {
        obj = bulletPool.GetObject();
        obj.transform.position = transform.position + offset;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, dir.Evaluate(deltaTime));
        obj.SetActive(true);
        Refire();
    }

    void Radial()
    {
        if (towardPlayer)
        {
            tPlayer = transform.position - target.transform.position;
            angle = Mathf.Atan2(tPlayer.y, tPlayer.x) * Mathf.Rad2Deg;
            dir.AddKey(deltaTime, angle + 90);
        }
        for (int i = 0; i < radNum; i++)
        {
            obj = bulletPool.GetObject();
            obj.transform.position = transform.position + offset;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, dir.Evaluate(deltaTime) + radMin + (i*(radMax - radMin)/(radNum-1)));
            obj.SetActive(true);
        }
        Refire();
    }

    void Chaos()
    {
        if (towardPlayer)
        {
            tPlayer = transform.position - target.transform.position;
            angle = Mathf.Atan2(tPlayer.y, tPlayer.x) * Mathf.Rad2Deg;
            dir.AddKey(deltaTime, angle + 90);
        }
        obj = bulletPool.GetObject();
        obj.transform.position = transform.position + offset;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, dir.Evaluate(deltaTime) + chaMin + (Random.value * (chaMax - chaMin)));
        obj.SetActive(true);
        Refire();
    }

    void Refire()
    {
        if (((int)freqType & 4) != 4)
        {
            shotCount++;
            if(shotCount > shotMax && shotMax != 0)
            {
                shotCount = 0;
                CancelInvoke();
                InvokeRepeating(functionName, repDelay, repFreq);
            }
        }
    }

    void OnDisable()
    {
        if (((int)freqType & 2) == 2)
        {
            if (((int)posType & 1) == 1)
            {
                Forward();
            }
        }
        CancelInvoke();
    }
}
