using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Range(0, 20)] public int Velocity;
    [Range(0, 90)] public int Angle;
    [Range(-1f, 1f)] public float Wind;
    public GameObject ProjectilePrefab;
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Instantiate(ProjectilePrefab, GameObject.Find("Column").transform, false);
	    }
	}
}