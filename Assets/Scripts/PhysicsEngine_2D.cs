using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine_2D : MonoBehaviour
{
    [Range(0f, 10f)] public float Mass;
    public Vector3 Velocity;
    private Vector3 _acceleration;
    private Cannon _cannon;
    private EnvironmentForces _forces;
    public ProjectileType ProjectileType;

    private void Awake()
    {
        Velocity = Vector3.zero;
        _acceleration = Vector3.zero;
    }

    // Use this for initialization
	private void Start ()
	{
	    _cannon = GameObject.Find(ProjectileType.ToString() + ("Cannon")).GetComponent<Cannon>();
	    _forces = GameObject.Find("Environment Forces").GetComponent<EnvironmentForces>();
        // Compoenent Initial velocity
        // Vx = Vi * Cos(Theta)
        // Vy = Vi * Sin(Theta)
	    Velocity.x = _cannon.Velocity * Mathf.Cos(Mathf.Deg2Rad * _cannon.Angle);
	    Velocity.y = _cannon.Velocity * Mathf.Sin(Mathf.Deg2Rad * _cannon.Angle);
	    if (ProjectileType == ProjectileType.Goat)
	    {
	        Velocity.x = -Velocity.x;
	    }
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    
	    _acceleration.y -= Mass * _forces.Gravity * Time.deltaTime;
	    if (_forces.Wind > 0 || _forces.Wind < 0)
	    {
	        if (ProjectileType == ProjectileType.Ball)
	            _acceleration += Velocity * _forces.Wind * Time.deltaTime;
	        else
                _acceleration -= Velocity * _forces.Wind * Time.deltaTime;
            
        }
	    if (_forces.AirResistance > 0 || _forces.AirResistance < 0)
	    {
	        _acceleration -= Velocity * _forces.AirResistance * Time.deltaTime;
	    }
	    Velocity += _acceleration * Time.deltaTime;
	    transform.position += Velocity * Time.deltaTime;
        if (transform.position.x > 25f || transform.position.y > 25f || transform.position.x < -25f || transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
