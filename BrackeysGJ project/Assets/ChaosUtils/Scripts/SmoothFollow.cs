using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public bool staticCamera;
    public Transform[] toFollow;
    public float moveTime;
    private Vector3 center;
    private Vector3 velosity;
    private float z;
    // Start is called before the first frame update
    void Start()
    {
        z = transform.position.z;
        velosity = Vector3.zero;
        center = CenteralPosition();
    }

    private Vector3 CenteralPosition()
    {
        Vector3 output = toFollow[0].position;
        foreach (Transform t in toFollow)
        {
            output = Vector3.Lerp(t.position, output, 0.5f);
        }
        output = new Vector3(output.x, output.y, z);
        return output;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (staticCamera) return;
        center = CenteralPosition();
        transform.position = Vector3.SmoothDamp(transform.position, center, ref velosity, moveTime);
        if (transform.position.y < -3.5f)
        {
            transform.position = new Vector3(transform.position.x,-3.5f,transform.position.z);
        }
    }
}
