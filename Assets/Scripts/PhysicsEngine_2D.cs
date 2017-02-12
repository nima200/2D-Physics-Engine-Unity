using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine_2D : MonoBehaviour
{
    [Range(0f, 20f)] public float Gravity;
    [Range(0f, 10f)] public float Mass;
    public Vector3 Velocity = Vector3.zero;
    private Vector3 _acceleration = Vector3.zero;
    private Cannon _cannon;

	// Use this for initialization
	void Start ()
	{
	    _cannon = GameObject.Find("Cannon").GetComponent<Cannon>();

        // Compoenent Initial velocity
        // Vx = Vi * Cos(Theta)
        // Vy = Vi * Sin(Theta)
	    Velocity.x = _cannon.Velocity * Mathf.Cos(Mathf.Deg2Rad * _cannon.Angle);
	    Velocity.y = _cannon.Velocity * Mathf.Sin(Mathf.Deg2Rad * _cannon.Angle);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _acceleration.y -= Mass * Gravity * Time.deltaTime;
	    if (_cannon.Wind > 0 || _cannon.Wind < 0)
	    {
            _acceleration += Velocity * _cannon.Wind * Time.deltaTime;
        }
        
	    Velocity += _acceleration * Time.deltaTime;
	    transform.position += Velocity * Time.deltaTime;
	}
}
