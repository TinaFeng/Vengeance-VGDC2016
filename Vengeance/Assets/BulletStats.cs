using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Movement {Target = 1, Bounce = 2, Size = 4, Rotate = 8};
public enum Disable {Boundary = 1, Time = 2};

public class BulletStats : MonoBehaviour {

    //Consider Polar Coordinates around a spawn location

    //General Variables
    PoolManager poolManager;
    BulletStats bomb;
    BulletStats bossBomb;
    public ObjectPooler objectPool;
    public Disable disableType;
    public Movement moveType;
    bool start = true;
    Vector3 tempVector3;
    GameObject player;
    float dis;
    float disBomb;
    public float radius;

    //Rotation
    Quaternion rot;
    Vector3 dir;
    float angle;
    float startAngle;

    //Follow/Target Movement
    public string targetTag;
    GameObject target;

    //Variable Speed
    public AnimationCurve speedVarX;
    public AnimationCurve speedVarY;
    public AnimationCurve rotation;
    public AnimationCurve size;
    float sizeTemp;
    float aliveTime = -1;
    float deltaTime;

    //Bounce
    public int bounceMax = 0;
    public int bounceCount = 0;
    Resolution resolution;
    float screenRatio;
    float widthOrtho;

    //Disable Boundary TEMP
    public float boundaryRadius = -0.1f;

    //Disable Time
    public float disableTime;
    bool isBomb;
    bool isBossBomb;

    void Awake()
    {
        isBomb = (gameObject.tag == "Bomb");
        isBossBomb = (gameObject.tag == "BossBomb");
        if (!isBomb && bomb == null)
        {
            bomb = GameObject.FindGameObjectWithTag("Bomb").GetComponent<BulletStats>();
        }
        if (!isBossBomb && bossBomb == null)
        {
            bossBomb = GameObject.FindGameObjectWithTag("BossBomb").GetComponent<BulletStats>();
        }
        if (poolManager == null)
        {
            poolManager = GameObject.Find("ObjectPooling").GetComponent<PoolManager>();
        }
        if (((int)moveType & 1) == 1)
        {
            target = GameObject.FindGameObjectWithTag(targetTag);
        }
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void OnEnable()
    {
        
        if (poolManager != null && poolManager.destroyAll)
        {
            Disable();
        }
        if (((int)moveType & 1) == 1 && target != null)
        {
            dir = transform.position - target.transform.position;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rot = Quaternion.Euler(0, 0, angle + 90.0f);
            transform.rotation = rot;
        }
        if (((int)moveType & 2) == 2)
        {
            bounceCount = 0;
        }

        if (((int)moveType & 4) == 4)
        {
            sizeTemp = size.Evaluate(0);
            tempVector3.Set(sizeTemp, sizeTemp, sizeTemp);
            transform.localScale = tempVector3;
        }
        if (((int)moveType & 8) == 8)
        {
            startAngle = transform.eulerAngles.z;
        }
        if (((int)disableType & 2) == 2)
        {
            Invoke("Disable", disableTime);
        }
        if (isBomb)
        {
            transform.position = player.transform.position;
        }
        aliveTime = 0;
        resolution = Screen.currentResolution;
        rot = transform.rotation;
    }

    void FixedUpdate()
    {
        if (poolManager.destroyAll)
        {
            Disable();
        }
        //Bullet Movement
        rot = transform.rotation;
        deltaTime = Time.deltaTime;
        tempVector3.Set(speedVarX.Evaluate(aliveTime) * deltaTime, speedVarY.Evaluate(aliveTime) * deltaTime, 0);
        transform.position += rot * tempVector3;
        if (((int)moveType & 4) == 4)
        {
            sizeTemp = size.Evaluate(aliveTime);
            tempVector3.Set(sizeTemp, sizeTemp, sizeTemp);
            transform.localScale = tempVector3;
            if (!isBomb && !isBossBomb)
            {
                if (poolManager.bombActive && gameObject.tag != "playerBullet")
                {
                    disBomb = Vector3.Distance(player.transform.position, transform.position) - (radius * sizeTemp) - (bomb.radius * bomb.sizeTemp);
                }
                if (poolManager.bossBombActive)
                {
                    disBomb = Vector3.Distance(bossBomb.transform.position, transform.position) - (radius * sizeTemp) - (bossBomb.radius * bossBomb.sizeTemp);
                }
                dis = Vector3.Distance(player.transform.position, transform.position) - (radius * sizeTemp);
            }
        }
        else
        {
            if (!isBomb && !isBossBomb)
            {
                if (poolManager.bombActive && gameObject.tag != "playerBullet")
                {
                    disBomb = Vector3.Distance(player.transform.position, transform.position) - (radius) - (bomb.radius * bomb.sizeTemp);
                }
                if (poolManager.bossBombActive)
                {
                    disBomb = Vector3.Distance(bossBomb.transform.position, transform.position) - (radius) - (bossBomb.radius * bossBomb.sizeTemp);
                }
                dis = Vector3.Distance(player.transform.position, transform.position) - (radius);
            }
        }
        if (((int)moveType & 8) == 8)
        {
            rot = Quaternion.Euler(0, 0, startAngle + rotation.Evaluate(aliveTime));
            transform.rotation = rot;
        }
        if (dis < 0 && !isBomb && !isBossBomb && gameObject.tag != "playerBullet" && player.GetComponent<PlayerController>().iFrames == false /*only deal damage if player isn't invuln*/) //Utilize this for bomb
        {
            player.GetComponent<PlayerController>().lives--;
            player.GetComponent<PlayerController>().updateLivesText();
            Disable();
            if (player.GetComponent<PlayerController>().lives > 0) player.GetComponent<PlayerController>().playerDamaged(); //If the player isn't dead, flash sprit and give 1 sec invuln
            else player.GetComponent<PlayerController>().gameOver(); //end the game until its reset
        }
        if (poolManager.bombActive || poolManager.bossBombActive)
        {
            if (disBomb < 0)
            {
                Disable();
            }
        }
        tempVector3 = transform.position;
        if (((int)moveType & 2) == 2 && bounceCount < bounceMax)
        {
            if (tempVector3.x + radius > 10 || tempVector3.x - radius < -10)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -transform.rotation.eulerAngles.z);
                bounceCount++;
            }
            if (bounceCount < bounceMax && (tempVector3.y + radius > 15))
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180 - transform.rotation.eulerAngles.z);
                bounceCount++;
            }
        }
        if (tempVector3.x > 11 + radius || tempVector3.x < -11 - radius)
        {
            Disable();
        }
        if (tempVector3.y > 16 + radius || tempVector3.y < -16 - radius)
        {
            Disable();
        }
        aliveTime += deltaTime;
    }

    void Disable()
    {
        if (!isBomb && !isBossBomb)
            gameObject.SetActive(false);
        else
        {
            this.enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            if(isBomb)
                poolManager.bombActive = false;
            if (isBossBomb)
                poolManager.bossBombActive = false;
        }
    }

    void OnDisable()
    {
        CancelInvoke();
        if (!isBomb && !isBossBomb)
            gameObject.GetComponent<BulletStats>().objectPool.deactivatedObjects.Push(gameObject);
    }
}
