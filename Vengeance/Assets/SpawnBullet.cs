using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position {Forward = 1, Radial = 2, Chaos = 4};
public enum Frequency {Repeating = 1, Disable = 2};

public class SpawnBullet : MonoBehaviour {

    PoolManager poolManager;
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
    float aliveTime;

    //Radial
    public float radMin;
    public float radMax;
    public int radNum;

    //Chaos
    public float chaMin;
    public float chaMax;

    //Repeating
    public float startDelay;
    public float repFreq;
    public float repDelay;
    public int shotMax;
    bool startWait;
    bool delayWait;
    int shotCount = 0;

	void OnEnable () {
        aliveTime = 0;
        deltaTime = 0;
        delayWait = false;
        startWait = true;
        if (poolManager == null)
        {
            poolManager = GameObject.Find("ObjectPooling").GetComponent<PoolManager>();
        }
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
    }
	
    void FixedUpdate()
    {
        if (!poolManager.timeStop)
        {
            if (poolManager.timeSlow)
                deltaTime += Time.deltaTime * poolManager.timeSlowAmount;
            else
                deltaTime += Time.deltaTime;
            aliveTime += deltaTime;
        }
        if (((int)freqType & 2) != 2)
        {
            if (startWait)
            {
                if(deltaTime >= startDelay)
                {
                    startWait = false;
                    deltaTime = repFreq;
                }
            }
            if(!startWait)
            {
                if (delayWait)
                {
                    if (deltaTime >= repDelay)
                    {
                        delayWait = false;
                        deltaTime = repFreq;
                    }
                }
                if (!delayWait)
                {
                    if(deltaTime >= repFreq)
                    {
                        Invoke(functionName, 0);
                        deltaTime = 0;
                    }
                }
            }
        }

    }
	void Forward()
    {
        obj = bulletPool.GetObject();
        obj.transform.position = transform.position + offset;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, dir.Evaluate(aliveTime));
        obj.SetActive(true);
        Refire();
    }

    void Radial()
    {
        if (towardPlayer)
        {
            tPlayer = transform.position - target.transform.position;
            angle = Mathf.Atan2(tPlayer.y, tPlayer.x) * Mathf.Rad2Deg;
            dir.AddKey(aliveTime, angle + 90);
        }
        for (int i = 0; i < radNum; i++)
        {
            obj = bulletPool.GetObject();
            obj.transform.position = transform.position + offset;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, dir.Evaluate(aliveTime) + radMin + (i*(radMax - radMin)/(radNum-1)));
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
            dir.AddKey(aliveTime, angle + 90);
        }
        obj = bulletPool.GetObject();
        obj.transform.position = transform.position + offset;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, dir.Evaluate(aliveTime) + chaMin + (Random.value * (chaMax - chaMin)));
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
                delayWait = true;
                deltaTime = 0;
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
