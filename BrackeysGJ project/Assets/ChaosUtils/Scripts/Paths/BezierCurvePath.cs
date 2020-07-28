using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurvePath : Path
{
    public override void OnRemoveSegment()
    {
        if (ControllPoints.Count <= 4) return;
        base.OnRemoveSegment();
    }
    public override void OnAddSegment(Vector2 pos)
    {
        ControllPoints.Add(ControllPoints[ControllPoints.Count - 1] * 2 - ControllPoints[ControllPoints.Count - 2]);
        ControllPoints.Add((ControllPoints[ControllPoints.Count - 1]) * 0.5f);
        ControllPoints.Add(pos);
    }
    public override void OnMovePoint(int index, Vector2 pos)
    {
        Vector2 deltaMove = pos - ControllPoints[index];
        ControllPoints[index] = pos;
        if (index % 3 == 0)
        {
            if (index + 1 < ControllPoints.Count) ControllPoints[index + 1] += deltaMove;
            if (index - 1 > 0) ControllPoints[index - 1] += deltaMove;
        }
        if (index % 3 == 1)
        {
            if (index - 2 > 0) ControllPoints[index - 2] = ControllPoints[index - 1] * 2f - ControllPoints[index];
        }
        if (index % 3 == 2)
        {
            if (index + 2 < ControllPoints.Count) ControllPoints[index + 2] = ControllPoints[index + 1] * 2f - ControllPoints[index];
        }
    }

    public override Vector2 GetPosition(float t)
    {
        float localT = t * ((ControllPoints.Count-1)/3);
        int segment = (int)localT;
        segment = Mathf.Min(segment, (ControllPoints.Count - 1) / 3 - 1);
        t = localT - segment;
        int i = segment*3;
        Vector2 newpoint = Mathf.Pow(1 - t, 3) * ControllPoints[i] +
                3 * Mathf.Pow(1 - t, 2) * t * ControllPoints[i + 1] +
                3 * Mathf.Pow(t, 2) * (1 - t) * ControllPoints[i + 2] +
                Mathf.Pow(t, 3) * ControllPoints[i + 3];

        return newpoint;
    }

}
