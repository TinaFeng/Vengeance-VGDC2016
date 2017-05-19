using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Movement {Target = 1, Bounce = 2, Size = 4, Rotate = 8};
public enum Disable {Boundary = 1, Time = 2};

public class BulletStats : MonoBehaviour {

    //Consider Polar Coordinates around a spawn location

    //General Variables
    public ObjectPooler objectPool;
    public Disable disableType;
    public Movement moveType;
    bool start = true;
    Vector3 tempVector3;
    GameObject player;
    float dis;
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

    void OnEnable()
    {
        if (aliveTime < 0 && ((int)moveType & 1) == 1)
        {
            target = GameObject.FindGameObjectWithTag(targetTag);
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
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
        aliveTime = 0;
        resolution = Screen.currentResolution;
        rot = transform.rotation;
    }

    void FixedUpdate()
    {
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
            dis = Vector3.Distance(player.transform.position, transform.position) - (radius * sizeTemp); //doesnt work on nonsphere hitboxes
        }
        else
        {
            dis = Vector3.Distance(player.transform.position, transform.position) - radius;
        }
        if (((int)moveType & 8) == 8)
        {
            rot = Quaternion.Euler(0, 0, startAngle + rotation.Evaluate(aliveTime));
            transform.rotation = rot;
        }
        if (dis < 0) //Utilize this for bomb
        {
            player.GetComponent<PlayerController>().lives--;
            player.GetComponent<PlayerController>().updateLivesText();
            Disable();
        }
        tempVector3 = transform.position;
        if (((int)moveType & 2) == 2 && bounceCount < bounceMax)
        {
            if(tempVector3.x + radius > 10 || tempVector3.x - radius < -10)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -transform.rotation.eulerAngles.z);
                bounceCount++;
            }
            if (bounceCount < bounceMax && (tempVector3.y + radius > 15 || tempVector3.y - radius < -15))
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
        script.radius = EditorGUILayout.FloatField("Hitbox Radius", script.radius);
        script.moveType = (Movement)EditorGUILayout.EnumMaskField("Move Type", script.moveType);
        if (((int)script.moveType & 1) == 1)
        {
            script.targetTag = EditorGUILayout.TextField("Target Tag", script.targetTag);
        }
        if (((int)script.moveType & 2) == 2)
        {
            script.bounceMax = EditorGUILayout.IntField("Max Bounces", script.bounceMax);
        }
        if (((int)script.moveType & 4) == 4)
        {
            script.size = EditorGUILayout.CurveField("Size Curve", script.size);
        }
        if (((int)script.moveType & 8) == 8)
        {
            script.rotation = EditorGUILayout.CurveField("Rotation Curve", script.rotation);
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