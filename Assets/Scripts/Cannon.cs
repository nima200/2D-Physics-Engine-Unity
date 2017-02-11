using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Range(0, 100)] public int velocity;
    [Range(0, 90)] public int angle;
    [Range(-0.5f, 0.5f)] public float wind;
    public GameObject projectilePrefab;
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Instantiate(projectilePrefab, GameObject.Find("Column").transform, false);
	    }
	}
}