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
    public Mountain MountainToCollide;
    public Ground GroundToCollide;
    public bool MountainLeft;
    public bool MountainMiddle;
    public bool MountainRight;
    public List<Vector3> _meshVertices;
    private List<Vector3> _groundVertices;

    private void Awake()
    {
        Velocity = Vector3.zero;
        _acceleration = Vector3.zero;
        MountainToCollide = GameObject.FindWithTag("Mountain").GetComponent<Mountain>();
        GroundToCollide = GameObject.FindWithTag("Ground").GetComponent<Ground>();
        _meshVertices = new List<Vector3>();
        _groundVertices = new List<Vector3>();
        //// Calling onto function that extracts mountain mesh vertices if mountain is present
        if (MountainToCollide != null)
        {
            ExtractMountain();
        }
        // Calling onto function that extracts ground mesh vertices if ground is present
        if (GroundToCollide != null)
        {
            ExtractGround();
        }
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

        // Collision detection
	    if (MountainToCollide != null)
	    {
	        foreach (Vector3 t in _meshVertices)
	        {
                //Debug.Log(t);
	            if ((Vector3.Distance(t, transform.position) - 0.2f)  < 0.5f)
	            {
	                Debug.Log("I COLLIDE");
	            }
	        }
	    }
    }

    private void ExtractMountain()
    {
        // Extracting all mountain mesh vertices, for all present parts (Left/Middle/Right) of the mountain
        if (MountainLeft)
            foreach (Vector3 t in MountainToCollide.GetComponentInChildren<MountainLeft>().gameObject.GetComponent<MeshFilter>().mesh.vertices)
            {
                if (!_meshVertices.Contains(t))
                {
                    _meshVertices.Add(t);
                }
            }
        if (MountainMiddle)
            foreach (Vector3 t in MountainToCollide.GetComponentInChildren<MountainMiddle>().gameObject
                .GetComponent<MeshFilter>()
                .mesh.vertices)
            {
                if (!_meshVertices.Contains(t))
                {
                    _meshVertices.Add(t);
                }
            }
        if (MountainRight)
            foreach (Vector3 t in MountainToCollide.GetComponentInChildren<MountainRight>().gameObject
                .GetComponent<MeshFilter>()
                .mesh.vertices)
            {
                if (!_meshVertices.Contains(t))
                {
                    _meshVertices.Add(t);
                }
            }
    }

    private void ExtractGround()
    {
        // Extracting all ground mesh vertices.
        foreach (Vector3 t in GroundToCollide.gameObject.GetComponent<MeshFilter>().mesh.vertices)
        {
            _groundVertices.Add(t);
        }
    }
}
