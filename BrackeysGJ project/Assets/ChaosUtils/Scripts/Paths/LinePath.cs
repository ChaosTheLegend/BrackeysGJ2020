using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePath : Path
{
    public override void OnMovePoint(int id,Vector2 pos)
    {
        ControllPoints[id] = pos;
    }
    public override void OnAddSegment(Vector2 pos)
    {
        ControllPoints.Add(pos);
    }

    public override void OnRemoveSegment()
    {
        if (ControllPoints.Count <= 2) return;
        ControllPoints.RemoveAt(ControllPoints.Count - 1);
    }

    public override Vector2 GetPosition(float t)
    {
        float localT = t * (ControllPoints.Count-1);
        int segment = (int)localT;
        segment = Mathf.Min(segment, ControllPoints.Count - 2);
        Vector2 pos = Vector2.Lerp(ControllPoints[segment], ControllPoints[segment + 1], localT - segment);
        return pos;
    }
}
