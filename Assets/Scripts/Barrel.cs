using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Cannon cannon;

	// Use this for initialization
	void Start ()
	{
	    cannon = GameObject.Find("Cannon").GetComponent<Cannon>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.rotation = Quaternion.Euler(0f, 0f, cannon.angle-90f);
	}
}
