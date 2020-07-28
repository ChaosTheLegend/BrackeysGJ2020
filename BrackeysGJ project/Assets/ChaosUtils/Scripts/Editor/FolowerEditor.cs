using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathFolower))]
public class FolowerEditor : Editor
{
    PathFolower folower;

    private void OnEnable()
    {
        folower = (PathFolower)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Start Folowing"))
        {
            folower.StartFollow();
        }
    }
}
