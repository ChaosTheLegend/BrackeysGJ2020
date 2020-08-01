using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Path : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Vector2> ControllPoints = new List<Vector2> { Vector2.zero, Vector2.zero + Vector2.left * 1f, Vector2.zero + Vector2.left * 2f, Vector2.zero + Vector2.left * 3f };
    private void Awake() => OnAwake();
    public virtual void OnAwake() { }

    public virtual void OnMovePoint(int id, Vector2 pos)
    {
        ControllPoints[id] = pos;
    }
    public virtual void OnAddSegment(Vector2 pos)
    {
        ControllPoints.Add(pos);
    }

    public virtual void OnRemoveSegment()
    {
        ControllPoints.RemoveAt(ControllPoints.Count - 1);
    }

    public abstract Vector2 GetPosition(float t);
    
}
