using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathManager myScript = (PathManager)target;
        //if (GUILayout.Button("StartLeftPath"))
        //{
        //    myScript.StartLeftPath();
        //}

        //if (GUILayout.Button("StartRightPath"))
        //{
        //    myScript.StartRightPath();
        //}
    }
}
