using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BezierCurvePath))]
public class BezierEditor : PathEditor
{

    public override void OnDrawPoints()
    {
        for (int i = 0; i < path.ControllPoints.Count; i++)
        {
            Handles.color = Color.blue;
            if (i % 3 == 0)
            {
                if (i + 1 != path.ControllPoints.Count) Handles.DrawLine(path.ControllPoints[i], path.ControllPoints[i + 1]);
                if (i - 1 > 0) Handles.DrawLine(path.ControllPoints[i], path.ControllPoints[i - 1]);
                Handles.color = Color.red;
            }
            Vector2 point = path.ControllPoints[i];

            point = Handles.FreeMoveHandle(point, Quaternion.identity, HandleUtility.GetHandleSize(point) * 0.1f, Vector3.one, Handles.SphereHandleCap);
            if (point != path.ControllPoints[i]) path.OnMovePoint(i, point);

        }

    }

}
