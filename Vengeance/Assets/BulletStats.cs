using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Movement {Straight = 1, Target = 2, Follow = 4, VariableSpeed = 8, Bounce = 16};
public enum Disable {Boundary = 1, Time = 2, Explosion = 4};

public class BulletStats : MonoBehaviour {

    //Consider Polar Coordinates around a spawn location

    //General Variables
    public ObjectPooler objectPool;
    public float speed;
    public Disable disableType;
    public Movement moveType;
    Quaternion startRotation;
    
    //Rotation
    Quaternion rot;
    Vector3 dir;
    Vector3 rotPos;
    Vector3 velocity;
    float angle;

    //Player Tag and Gameobjects


    //Follow/Target Movement
    public float followTime = 5f;
    float startFollowTime;
    public string followTargetTag;
    GameObject followTarget;
    bool startTarget;

    //Variable Speed
    public float speedVar;
    float startSpeed;

    //Bounce
    public int bounceMax;
    public float bounceRadius;
    int bounceCount = 0;

    //Disable Boundary
    public float boundaryRadius = -0.1f;
    Vector3 disablePos;
    float screenRatio;
    float widthOrtho;

    //Disable Time
    public float disableTime;

    void Start()
    {
        startRotation = transform.rotation;
        if (((int)moveType & 2) == 2 || ((int)moveType & 4) == 4)
        {
            followTarget = GameObject.FindGameObjectWithTag(followTargetTag);
        }
        if (((int)moveType & 8) == 8)
        {
            startSpeed = speed;
        }
    }

    void OnEnable()
    {
        if (((int)moveType & 2) == 2)
        {
            startTarget = true;
        }
        if (((int)moveType & 4) == 4)
        {
            startFollowTime = Time.time;
        }
        if (((int)moveType & 8) == 8)
        {
            speed = startSpeed;
        }
        if (((int)moveType & 16) == 16)
        {
            bounceCount = 0;
        }

        if (((int)disableType & 2) == 2)
        {
            Invoke("Disable", disableTime);
        }
    }

    void FixedUpdate()
    {
        //Bullet Movement
        if (((int)moveType & 1) == 1)
        {
            rot = transform.rotation;
            rotPos = transform.position;
            velocity.Set(0, speed * Time.deltaTime, 0);
            rotPos += rot * velocity;
            transform.position = rotPos;
        }
        
        if(((int)moveType & 2) == 2)
        {
            rot = transform.rotation;
            if (startTarget)
            {
                dir = transform.position - followTarget.transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rot = Quaternion.Euler(0, 0, angle + 90.0f);
                transform.rotation = rot;
                startTarget = false;
            }
            rotPos = transform.position;
            velocity.Set(0, speed * Time.deltaTime, 0);
            rotPos += rot * velocity;
            transform.position = rotPos;
        }

        if (((int)moveType & 4) == 4)
        {
            rot = transform.rotation;
            if (followTarget != null && Time.time - startFollowTime < followTime)
            {
                dir = transform.position - followTarget.transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rot = Quaternion.Euler(0, 0, angle + 90.0f);
                transform.rotation = rot;
            }
            rotPos = transform.position;
            velocity.Set(0, speed * Time.deltaTime, 0);
            rotPos += rot * velocity;
            transform.position = rotPos;
        }

        if (((int)moveType & 8) == 8)
        {
            speed += speedVar;
        }

        if (((int)moveType & 16) == 16)
        {
            if (bounceCount < bounceMax)
            {
                if (transform.position.y + bounceRadius >= Camera.main.orthographicSize)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f - transform.rotation.z);
                    bounceCount++;
                }
                if (transform.position.y - bounceRadius <= -Camera.main.orthographicSize)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f - transform.rotation.z);
                    bounceCount++;
                }
                screenRatio = (float)Screen.width / (float)Screen.height;
                widthOrtho = Camera.main.orthographicSize * screenRatio;
                if (transform.position.x + bounceRadius >= widthOrtho)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 135f - transform.rotation.z);
                    bounceCount++;
                }
                if (transform.position.x - bounceRadius <= -widthOrtho)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 225f - transform.rotation.z);
                    bounceCount++;
                }
            }
        }

                    //Disable Bullets
        if (((int)disableType & 1) == 1)
        {
            disablePos = transform.position;
            screenRatio = (float)Screen.width / (float)Screen.height;
            widthOrtho = Camera.main.orthographicSize * screenRatio;
            if (disablePos.y + boundaryRadius > Camera.main.orthographicSize || disablePos.y - boundaryRadius < -Camera.main.orthographicSize || disablePos.x + boundaryRadius > widthOrtho || disablePos.x - boundaryRadius < -widthOrtho)
            {
                gameObject.SetActive(false);
            }
        }

    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        transform.rotation = startRotation;
        CancelInvoke();
        gameObject.GetComponent<BulletStats>().objectPool.deactivatedObjects.Push(gameObject);
    }
}



[CustomEditor(typeof(BulletStats))]
public class BulletStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var script = target as BulletStats;
        script.speed = EditorGUILayout.FloatField("Speed", script.speed);
        //Movement Options
        script.moveType = (Movement)EditorGUILayout.EnumMaskField("Move Type", script.moveType);
        if (((int)script.moveType & 2) == 2)
        {
            script.followTargetTag = EditorGUILayout.TextField("Target Tag", script.followTargetTag);
        }
        if (((int)script.moveType & 4) == 4)
        {
            script.followTime = EditorGUILayout.FloatField("Follow Time", script.followTime);
            script.followTargetTag = EditorGUILayout.TextField("Follow Target Tag", script.followTargetTag);
        }
        if (((int)script.moveType & 8) == 8)
        {
            script.speedVar = EditorGUILayout.FloatField("Speed Variable", script.speedVar);
        }
        if(((int)script.moveType & 16) == 16)
        {
            script.bounceMax = EditorGUILayout.IntField("Max Bounces", script.bounceMax);
            script.bounceRadius = EditorGUILayout.FloatField("Bounce Radius", script.bounceRadius);
        }

        //Disable Options
        script.disableType = (Disable)EditorGUILayout.EnumMaskField("Disable Type", script.disableType);
        if (((int)script.disableType & 1) == 1) {
            script.boundaryRadius = EditorGUILayout.FloatField("Boundary Radius", script.boundaryRadius);
        }
        if (((int)script.disableType & 2) == 2)
        {
            script.disableTime = EditorGUILayout.FloatField("Time to Disable", script.disableTime);
        }
    }
}