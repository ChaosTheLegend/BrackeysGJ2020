using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Path))]
public abstract class PathEditor : Editor
{
    public Path path;
    private void OnEnable()
    {
        path = (Path)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Segment"))
        {
            path.OnAddSegment(Vector2.zero);
        }
        if (GUILayout.Button("Remove Segment"))
        {
            path.OnRemoveSegment();
        }
    }

    public virtual void OnDrawPoints()
    {
        for (int i = 0; i < path.ControllPoints.Count; i++)
        {
            Handles.color = Color.red;
            if (i == 0 || i == path.ControllPoints.Count - 1) Handles.color = Color.blue;
            Vector2 point = path.ControllPoints[i];
            point = Handles.FreeMoveHandle(point, Quaternion.identity, HandleUtility.GetHandleSize(point) * 0.1f, Vector3.one, Handles.SphereHandleCap);
            if (path.grid)
            {
                point = new Vector3((int) (point.x / 0.5f) * 0.5f, (int) (point.y / 0.5f) * 0.5f, 0f);
            }
            if (point != path.ControllPoints[i]) path.OnMovePoint(i, point);
        }
    }

    public virtual void OnDrawPath()
    {
        Handles.color = Color.yellow;
        Vector2 lastpos = path.GetPosition(0f);
        for (float t = 0; t < 1.01f; t += 0.02f)
        {
            Vector2 newpos = path.GetPosition(t);
            Handles.DrawLine(lastpos, newpos);
            lastpos = newpos;
        }
    }
    public void OnSceneGUI()
    {
        //Drawing Path
        OnDrawPath();
        OnDrawPoints();
    }
}
