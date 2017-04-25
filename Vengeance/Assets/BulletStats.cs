using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Movement {Target = 1, Bounce = 2};
public enum Disable {Boundary = 1, Time = 2};

public class BulletStats : MonoBehaviour {

    //Consider Polar Coordinates around a spawn location

    //General Variables
    public ObjectPooler objectPool;
    public float speedX;
    public float speedY;
    public Disable disableType;
    public Movement moveType;
    bool start = true;


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
    public AnimationCurve speedVarX;
    public AnimationCurve speedVarY;
    float speedTime;
    float timeChange;

    //Bounce
    public int bounceMax;
    public float bounceRadius;
    int bounceCount = 0;
    Resolution resolution;

    //Disable Boundary
    public float boundaryRadius = -0.1f;
    Vector3 disablePos;
    float screenRatio;
    float widthOrtho;

    //Disable Time
    public float disableTime;

    void OnEnable()
    {
        speedTime = 0;
        resolution = Screen.currentResolution;
        if (start)
        {
            if (((int)moveType & 1) == 1)
            {
                followTarget = GameObject.FindGameObjectWithTag(followTargetTag);
            }
            start = false;
        }
        if (((int)moveType & 1) == 1)
        {
            if (followTarget != null)
            {
                dir = transform.position - followTarget.transform.position;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rot = Quaternion.Euler(0, 0, angle + 90.0f);
                transform.rotation = rot;
            }
        }
        if (((int)moveType & 2) == 2)
        {
            bounceCount = 0;
        }

        if (((int)disableType & 2) == 2)
        {
            Invoke("Disable", disableTime);
        }
        rot = transform.rotation;
    }

    void FixedUpdate()
    {
        //Bullet Movement
        timeChange = Time.deltaTime;
        speedX = speedVarX.Evaluate(speedTime);
        speedY = speedVarY.Evaluate(speedTime);
        speedTime += timeChange;
        velocity.Set(0, speedY * timeChange, 0);
        transform.position += rot * velocity;

        //Weirdly changes rotation ATM
        if (((int)moveType & 2) == 2)
        {
            if (bounceCount < bounceMax)
            {
                if(!resolution.Equals(Screen.currentResolution))
                {
                    screenRatio = (float)Screen.width / (float)Screen.height;
                    widthOrtho = Camera.main.orthographicSize * screenRatio;
                }

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

        //MOVE TO A COLLIDER NOT ON THE BULLET
        //Disable Bullets
        //if (((int)disableType & 1) == 1)
        //{
        //    disablePos = transform.position;
        //    screenRatio = (float)Screen.width / (float)Screen.height;
        //    widthOrtho = Camera.main.orthographicSize * screenRatio;
        //    if (disablePos.y + boundaryRadius > Camera.main.orthographicSize || disablePos.y - boundaryRadius < -Camera.main.orthographicSize || disablePos.x + boundaryRadius > widthOrtho || disablePos.x - boundaryRadius < -widthOrtho)
        //    {
        //        gameObject.SetActive(false);
        //    }
        //}
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
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

        //Movement Options
        script.speedVarX = EditorGUILayout.CurveField("Speed Curve X", script.speedVarX);
        script.speedVarY = EditorGUILayout.CurveField("Speed Curve Y", script.speedVarY);
        script.moveType = (Movement)EditorGUILayout.EnumMaskField("Move Type", script.moveType);
        if (((int)script.moveType & 1) == 1)
        {
            script.followTargetTag = EditorGUILayout.TextField("Target Tag", script.followTargetTag);
        }
        if (((int)script.moveType & 2) == 2)
        {
            script.bounceMax = EditorGUILayout.IntField("Max Bounces", script.bounceMax);
            script.bounceRadius = EditorGUILayout.FloatField("Bounce Radius", script.bounceRadius);
        }

        //Disable Options
        script.disableType = (Disable)EditorGUILayout.EnumMaskField("Disable Type", script.disableType);
        if (((int)script.disableType & 1) == 1)
        {
            script.boundaryRadius = EditorGUILayout.FloatField("Boundary Radius", script.boundaryRadius);
        }
        if (((int)script.disableType & 2) == 2)
        {
            script.disableTime = EditorGUILayout.FloatField("Time to Disable", script.disableTime);
        }
    }
}