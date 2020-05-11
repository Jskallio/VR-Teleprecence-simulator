using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Human))]
public class HumanEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Human myScript = (Human)target;
        if (GUILayout.Button("Add waypoint"))
        {
            myScript.waypoints.Add(myScript.target.position);
        }
    }
}
