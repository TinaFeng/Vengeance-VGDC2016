using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CurveEditorWindow : EditorWindow
{
    public GameObject gameObject;
    AnimationCurve x;
    AnimationCurve y;

    [MenuItem("Window/My Window")]

    
    
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CurveEditorWindow));
    }

    void OnGUI()
    {
        gameObject = Selection.activeGameObject;
        if (gameObject != null)
        {
            x = /*EditorGUILayout.CurveField("Animation on Z",*/ gameObject.GetComponent<CurveMovement>().locX;
            y = /*EditorGUILayout.CurveField("Animation on Z",*/ gameObject.GetComponent<CurveMovement>().locY;
            EditorGUI.CurveField(new Rect(0, 0, 500, 500), x);
            EditorGUI.CurveField(new Rect(0, 0, 500, 500), y);

        }
    }
}
