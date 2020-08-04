using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathFolower : MonoBehaviour
{
    [SerializeField] public Path path;
    [SerializeField] public float speed;
    public enum EndActions { Stop = 0, Restart = 1, GoBack = 2 }
    [SerializeField] public EndActions onEnd;

    private float tParam = 0f;
    private float Tstart;
    private float Tend;
    private float deltaT;
    private float _dir;
    private bool allowCorutine = true;
    private bool isFolowing = false;

    public UnityEvent onPathEnd;

    public void UpdateSpeed()
    {
        deltaT = speed*_dir;
    }

    public void SetT(float t)
    {
        tParam = t;
    }

    public float GetDir()
    {
        return _dir;
    }
    public void StartFollow()
    {
        _dir = 1f;
        Tstart = 0f;
        Tend = 1f;
        tParam = Tstart;
        deltaT = speed;
        isFolowing = true;
    }
    
    private void EndAction()
    {
        if ((int)onEnd == 0)
        {
            isFolowing = false;
            return;
        }
        if ((int)onEnd == 1)
        {
            isFolowing = true;
            tParam = Tstart;
            return;
        }
        if ((int)onEnd == 2)
        {
            isFolowing = true;
            tParam = Tend;
            float placeholder = Tstart;
            Tstart = Tend;
            Tend = placeholder;
            _dir *= -1f;
            deltaT = speed*_dir;
            return;
        }
    }
    private IEnumerator FollowPath()
    {
        allowCorutine = false;
        tParam = Tstart;
        while (Mathf.Abs(Tend-tParam) > Mathf.Abs(deltaT)*0.05f)
        {
            transform.position = path.GetPosition(tParam);
            tParam += Time.deltaTime * deltaT;
            yield return new WaitForEndOfFrame();
        }
        allowCorutine = true;
        EndAction();
        onPathEnd?.Invoke();
    }

    void Update()
    {
        if (allowCorutine && isFolowing)
        {
            StartCoroutine(FollowPath());
        }
    }
}
