using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Cannon _cannon;
    private ProjectileType _projectileType;

	// Use this for initialization
	void Start ()
	{
	    _cannon = GetComponentInParent<Cannon>();
	    _projectileType = _cannon.MyProjectileType;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.rotation = _projectileType == ProjectileType.Ball
	        ? Quaternion.Euler(0f, 0f, _cannon.Angle - 90f)
	        : Quaternion.Euler(0f, 0f, 90f - _cannon.Angle);
	}
}
