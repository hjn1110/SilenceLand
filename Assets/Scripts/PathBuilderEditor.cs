using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathList))]
public class PathBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            //DrawDefaultInspector();

            PathList nodsList = (PathList)target;

            if(GUILayout.Button("Creat Nods"))
            {
                nodsList.CreatNods();
            }
            if (GUILayout.Button("Add"))
            {
                nodsList.Add();
            }
            if (GUILayout.Button("Clear"))
            {
                nodsList.Clear();
            }
        }
    }
}
