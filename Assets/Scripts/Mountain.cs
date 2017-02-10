using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : MonoBehaviour {
    // Global parameters for the mountain components to feed off of.
    public int Width;
    public int Height;
    [Range(1, 10)] public int recursionLevels;
    [Range(1, 50)] public int smoothness;
}
