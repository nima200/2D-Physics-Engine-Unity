using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentForces : MonoBehaviour
{
    [Range(0f, 20f)] public float Gravity;
    [Range(-1.0f, 1.0f)] public float Wind;
    [Range(0f, 1.0f)]public float AirResistance; 

    void Awake()
    {
        Gravity = 9.81f;
        AirResistance = 0.1f;
    }
}
