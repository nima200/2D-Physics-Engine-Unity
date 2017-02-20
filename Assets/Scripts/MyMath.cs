using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath {

    public static float PointLineDistance(Vector3 start, Vector3 end, Vector3 point)
    {
        var u = end - start;
        var v = point - start;
        var normal = Vector3.Cross(Vector3.forward, u);
        float uv = Vector3.Dot(u, v) / Vector3.SqrMagnitude(u);
        if (!(uv >= 0) || !(uv <= 1)) return Vector3.Distance(point, uv < 0 ? start : end);
        var projection = start + uv * u;

        return Vector3.Distance(point, projection);
    }	
}
