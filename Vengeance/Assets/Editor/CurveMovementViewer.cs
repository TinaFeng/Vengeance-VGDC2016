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
        positions = new Vector3[(int)script.timeToKill*10];
        for(int i = 0; i < (int)script.timeToKill * 10; i++)
        {
            positions[i] = new Vector3(script.locX.Evaluate(i * 0.1f), script.locY.Evaluate(i * 0.1f), 0f);
        }
        Handles.DrawAAPolyLine(positions);
    }
}
