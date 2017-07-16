using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnBullet))]
public class SpawnBulletEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var script = target as SpawnBullet;
        script.bulletPool = EditorGUILayout.ObjectField("Bullet Pool", script.bulletPool, typeof(ObjectPooler), true) as ObjectPooler;
        script.offset = EditorGUILayout.Vector3Field("Offset", script.offset);
        script.towardPlayer = EditorGUILayout.Toggle("Toward Player", script.towardPlayer);
        script.dir = EditorGUILayout.CurveField("Direction", script.dir);
        //Position Options
        script.posType = (Position)EditorGUILayout.EnumMaskField("Spawn Position Type", script.posType);
        if (((int)script.posType & 2) == 2)
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
            script.shotMax = EditorGUILayout.IntField("Burst Number", script.shotMax);
        }

    }
}
