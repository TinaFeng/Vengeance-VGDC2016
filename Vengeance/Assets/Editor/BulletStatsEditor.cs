using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
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