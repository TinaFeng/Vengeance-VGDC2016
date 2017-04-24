using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Position {Forward = 1, Radial = 2, Chaos = 4};
public enum Frequency {Repeating = 1, Disable = 2};

public class SpawnBullet : MonoBehaviour {

    public ObjectPooler bulletPool;
    public Position posType;
    public Frequency freqType;
    string functionName;
    GameObject obj;

    public Vector3 offset;
    public float dir;

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
	

	void OnEnable () {
		if(((int)posType & 1) == 1)
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
            InvokeRepeating(functionName, repFreq + repDelay, repFreq);
        }
    }
	
	void Forward()
    {
        obj = bulletPool.GetObject();
        obj.transform.position = transform.position + offset;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, dir);
        obj.SetActive(true);
    }

    void Radial()
    {
        for(int i = 0; i < radNum; i++)
        {
            obj = bulletPool.GetObject();
            obj.transform.position = transform.position + offset;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, dir + radMin + (i*(radMax - radMin)/(radNum-1)));
            obj.SetActive(true);
        }
    }

    void Chaos()
    {
        obj = bulletPool.GetObject();
        obj.transform.position = transform.position + offset;
        obj.transform.rotation = Quaternion.Euler(0f, 0f, dir + chaMin + (Random.value * (chaMax - chaMin)));
        obj.SetActive(true);
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


[CustomEditor(typeof(SpawnBullet))]
public class SpawnBulletEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var script = target as SpawnBullet;
        script.bulletPool = EditorGUILayout.ObjectField("Bullet Pool", script.bulletPool, typeof(ObjectPooler), true) as ObjectPooler;
        script.offset = EditorGUILayout.Vector3Field("Offset", script.offset);
        script.dir = EditorGUILayout.FloatField("Direction", script.dir);
        //Position Options
        script.posType = (Position)EditorGUILayout.EnumMaskField("Spawn Position Type", script.posType);
        if(((int)script.posType & 2) == 2)
        {
            script.radMin = EditorGUILayout.FloatField("Left Raidial", script.radMin);
            script.radMax = EditorGUILayout.FloatField("Right Raidial", script.radMax);
            script.radNum = EditorGUILayout.IntField("Number of Bullets", script.radNum);
        }
        if (((int)script.posType & 4) == 4)
        {
            script.chaMin = EditorGUILayout.FloatField("Left Raidial", script.chaMin);
            script.chaMax = EditorGUILayout.FloatField("Right Raidial", script.chaMax);
        }
        //Frequency Options
        script.freqType = (Frequency)EditorGUILayout.EnumMaskField("Frequency Type", script.freqType);
        if (((int)script.freqType & 1) == 1)
        {
            script.repFreq = EditorGUILayout.FloatField("Repeating Frequency", script.repFreq);
            script.repDelay = EditorGUILayout.FloatField("Delay to Start", script.repDelay);
        }
        
    }
}