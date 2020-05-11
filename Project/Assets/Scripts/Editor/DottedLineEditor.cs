using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CreateDottedLine))]
public class DottedLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CreateDottedLine myScript = (CreateDottedLine)target;
        if (GUILayout.Button("Add waypoint"))
        {
            myScript.points.Add(myScript.target.position);
        }
        if(GUILayout.Button("Reverse"))
        {
            myScript.points.Reverse();
        }
    }
}
