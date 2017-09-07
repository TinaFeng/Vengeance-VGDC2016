using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(CurveMovement))]
public class CurveMovementViewer: Editor {

    Vector3[] positions;

	public void OnSceneGUI()
    {
        var script = target as CurveMovement;
        int size;
        if(script.timeToKill != 0)
            size = (int)script.timeToKill*10;
        else
            size = 1000;
        positions = new Vector3[size];
        for (int i = 0; i < size; i++)
        {
            positions[i] = new Vector3(script.locX.Evaluate(i * 0.1f), script.locY.Evaluate(i * 0.1f), 0f);
        }
        Handles.DrawAAPolyLine(positions);
    }
}
