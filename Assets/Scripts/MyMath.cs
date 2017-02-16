using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath {

    public static float PointLineDistance(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 u = end - start;
        Vector3 v = point - start;
        float uv = Vector3.Dot(u, v) / Vector3.SqrMagnitude(u);
        if (uv >= 0 && uv <= 1)
        {
//            Debug.Log("CASE 1");
            Vector3 projection = start + uv * u;
            return Vector3.Distance(point, projection);

        }
        // if uv < 0, projection is on (-inf, start) interval, and if uv > 1 projection is on (end, +inf) interval
//        return uv < 0 ? Vector3.Distance(point, start) : Vector3.Distance(point, end);
        if (uv < 0)
        {
//            Debug.Log("CASE 2");
            return Vector3.Distance(point, start);
        }
        else
        {
//            Debug.Log("CASE 3");
            return Vector3.Distance(point, end);
        }
    }
	
}
