using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorDistance {

    public static void Echo(Vector3 a ,Vector3 b, int index1, int index2)
    {
        Debug.Log("Distance between vector " + index1 + " and vector " + index2 + ": " + Vector3.Distance(a, b));
    }
}
