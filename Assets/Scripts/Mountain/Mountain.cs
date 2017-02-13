using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour {
    // Global parameters for the mountain components to feed off of.
    public float Width;
    public int Height;
    [Range(1, 10)] public int RecursionLevels;
    [Range(1, 50)] public int Smoothness;

    private void Awake()
    {
        Width = Random.Range(10.0f, 12.0f);
    }
}
