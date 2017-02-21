using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindIndicator : MonoBehaviour
{

    public EnvironmentForces Forces;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.GetChild(0).transform.localScale = new Vector3(1f, Forces.Wind, 1f);
	}
}
