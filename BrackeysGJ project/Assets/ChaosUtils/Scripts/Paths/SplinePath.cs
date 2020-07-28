using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePath : Path
{
    [Range(0f,1f)]
    [SerializeField] public float alpha = 0.5f;


    public override void OnRemoveSegment()
    {
        if (ControllPoints.Count <= 4) return;
        base.OnRemoveSegment();
    }
    //Catmull-Rom algorythm
    public float GetT(float t, Vector2 A, Vector2 B)
    {
        float deltaX = B.x - A.x;
        float deltaY = B.y - A.y;
        float sqrDelataX = deltaX * deltaX;
        float sqrDelataY = deltaY * deltaY;


        float output = Mathf.Pow(sqrDelataX + sqrDelataY,alpha*0.5f);

        return output+t;
    }

    public override Vector2 GetPosition(float t)
    {
        float localT = t * (ControllPoints.Count-3);
        int segment = (int)localT;
        segment = Mathf.Min(segment, ControllPoints.Count - 4);
        segment = Mathf.Max(0, segment);
        t = localT - segment;
        

        float t0 = 0;
        float t1 = GetT(t0, ControllPoints[segment], ControllPoints[segment+1]);
        float t2 = GetT(t1, ControllPoints[segment+1], ControllPoints[segment+2]);
        float t3 = GetT(t2, ControllPoints[segment+2], ControllPoints[segment+3]);

        t = t1+ (t2 - t1)*t;

        Vector2 A1 = ((t1 - t) / (t1 - t0)) * ControllPoints[segment] + ((t - t0) / (t1 - t0)) * ControllPoints[segment+1];
        Vector2 A2 = ((t2 - t) / (t2 - t1)) * ControllPoints[segment+1] + ((t - t1) / (t2 - t1)) * ControllPoints[segment+2];
        Vector2 A3 = ((t3 - t) / (t3 - t2)) * ControllPoints[segment+2] + ((t - t2) / (t3 - t2)) * ControllPoints[segment+3];

        Vector2 B1 = ((t2 - t) / (t2 - t0)) * A1 + ((t - t0) / (t2 - t0)) * A2;
        Vector2 B2 = ((t3 - t) / (t3 - t1)) * A2 + ((t - t1) / (t3 - t1)) * A3;

        Vector2 C = ((t2 - t) / (t2 - t1)) * B1 + ((t - t1) / (t2 - t1)) * B2;

        return C;
    }




    public Vector4[] SolveSpline(List<Vector2> points)
    {
        Vector4 X = new Vector4(points[0].x, points[1].x, points[2].x, points[3].x);
        Vector4 Y = new Vector4(points[0].y, points[1].y, points[2].y, points[3].y);
        Matrix4x4 tMatrix = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int q = 0; q < 4; q++)
            {
                tMatrix[i, q] = Mathf.Pow((float)i/3f, q); 
            }
        }
        tMatrix = tMatrix.inverse;
        Vector4 Xcoeficients = tMatrix * X;
        Vector4 Ycoeficients = tMatrix * Y;
        return new Vector4[] {Xcoeficients, Ycoeficients};
    }
}
