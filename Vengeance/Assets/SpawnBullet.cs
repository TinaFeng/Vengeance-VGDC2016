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

    //Radial
    public float radMin;
    public float radMax;

    //Chaos

    //Repeating
    public float repFreq;
    public float repDelay;
	

	void OnEnable () {
		if(((int)posType & 1) == 1)
        {
            functionName = "Forward";
        }
        if (((int)freqType & 2) != 2)
        {
            InvokeRepeating(functionName, repFreq + repDelay, repFreq);
        }
    }
	
	void Forward()
    {
        GameObject obj = bulletPool.GetObject();
        obj.transform.position = transform.position;
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
        //Position Options
        script.posType = (Position)EditorGUILayout.EnumMaskField("Spawn Position Type", script.posType);
        if(((int)script.posType & 1) == 1)
        {

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