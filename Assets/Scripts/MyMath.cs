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

    // line segment intersection test
    // http://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/

    private static bool OnSegment(Vector3 p, Vector3 q, Vector3 r)
    {
        if (q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
        q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
            return true;

        return false;
    }

    private static int Orientation(Vector3 p, Vector3 q, Vector3 r)
    {
        int val = (int) ((q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y));
        if (val == 0) return 0;
        return (val > 0) ? 1 : 2;
    }

    public static bool CheckIntersect(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2)
    {
        int o1 = Orientation(p1, q1, p2);
        int o2 = Orientation(p1, q1, q2);
        int o3 = Orientation(p2, q2, p1);
        int o4 = Orientation(p2, q2, q1);
        if (o1 != o2 && o3 != o4)
        {
            return true;
        }
        if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

        if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

        if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

        return o4 == 0 && OnSegment(p2, q1, q2);
    }
}
