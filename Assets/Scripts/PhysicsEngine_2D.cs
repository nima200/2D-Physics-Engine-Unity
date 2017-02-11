using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine_2D : MonoBehaviour
{
    [Range(0f, 20f)] public float gravity;
    [Range(0f, 10f)] public float mass;
    public Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    private Cannon cannon;

	// Use this for initialization
	void Start ()
	{
	    cannon = GameObject.Find("Cannon").GetComponent<Cannon>();

        // Compoenent Initial velocity
        // Vx = Vi * Cos(Theta)
        // Vy = Vi * Sin(Theta)
	    velocity.x = cannon.velocity * Mathf.Cos(Mathf.Deg2Rad * cannon.angle);
	    velocity.y = cannon.velocity * Mathf.Sin(Mathf.Deg2Rad * cannon.angle);
	}
	
	// Update is called once per frame
	void Update ()
	{
	    acceleration.y -= mass * gravity * Time.deltaTime;
	    if (cannon.wind != 0)
	    {
            acceleration += velocity * cannon.wind * Time.deltaTime;
        }
        
	    velocity += acceleration * Time.deltaTime;
	    transform.position += velocity * Time.deltaTime;
	}
}
