using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsEngine_2D : MonoBehaviour
{
    [Range(0f, 10f)] public float Mass;
    [Range(1f, 200f)] public float Bounce;
    public Vector3 _velocity;
    private Vector3 _acceleration;
    private Cannon _cannon;
    private EnvironmentForces _forces;
    public ProjectileType ProjectileType;
    public Mountain MountainToCollide;
    public Ground GroundToCollide;
    public bool MountainLeft;
    public bool MountainMiddle;
    public bool MountainRight;
    private List<Vector3> _mountainVertices;
    private List<Vector3> _mountainTopVertices;
    private List<Vector3> _groundVertices;
    private bool _exploded;

    private void Awake()
    {
        _velocity = Vector3.zero;
        _acceleration = Vector3.zero;
        MountainToCollide = GameObject.FindWithTag("Mountain").GetComponent<Mountain>();
        GroundToCollide = GameObject.FindWithTag("Ground").GetComponent<Ground>();
        _mountainVertices = new List<Vector3>();
        _mountainTopVertices = new List<Vector3>();
        _groundVertices = new List<Vector3>();
        // Calling onto function that extracts mountain mesh vertices if mountain is present
        if (MountainToCollide != null)
        {
            ExtractMountain();
            ExtractMountainTop();
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
	    _velocity.x = _cannon.Velocity * Mathf.Cos(Mathf.Deg2Rad * _cannon.Angle);
	    _velocity.y = _cannon.Velocity * Mathf.Sin(Mathf.Deg2Rad * _cannon.Angle);
	    if (ProjectileType == ProjectileType.Goat)
	    {
	        _velocity.x = -_velocity.x;
	    }
	}

    private IEnumerator DestroyProjectile()
    {
        // Start animation right away but destroy after animation done
        // Animation duration = 2 seconds
        gameObject.GetComponent<Animator>().speed = 2f;
        gameObject.GetComponent<Animator>().enabled = true;
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	private void Update ()
	{
	    _acceleration.y -= Mass * _forces.Gravity * Time.deltaTime;
	    if (_forces.Wind > 0 || _forces.Wind < 0)
	    {
	        if (ProjectileType == ProjectileType.Ball)
	            _acceleration += _velocity * _forces.Wind * Time.deltaTime;
	        else
                _acceleration += -_velocity * _forces.Wind * Time.deltaTime;
        }
	    if (_forces.AirResistance > 0 || _forces.AirResistance < 0)
	    {
	        _acceleration -= _velocity * _forces.AirResistance * Time.deltaTime;
	    }
	    _velocity += _acceleration * Time.deltaTime;
	    transform.position += _velocity * Time.deltaTime;
        if (transform.position.x > 18f || transform.position.y > 18f || transform.position.x < -18f || transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
        
        // Collision detection
	    if (MountainToCollide != null)
	    {
            if (Bounce < 1 && ProjectileType == ProjectileType.Ball)
	        {
                // Coroutine to delay destruction
	            StartCoroutine(DestroyProjectile());
	        }
	        var mountainVertexPairs =
	            from vertex1 in _mountainVertices
	            from vertex2 in _mountainVertices
	            select new[] {vertex1, vertex2};
	        foreach (var pair in mountainVertexPairs)
	        {
	            if (MyMath.PointLineDistance(pair[0], pair[1], transform.GetComponentInChildren<Circle>().gameObject.transform.position) > 0.25f) continue;
                ResetAcceleration();
	            ResetVelocity();
//	            _velocity += new Vector3(((pair[1] - pair[0]).x - Vector3.zero.x), ((pair[1] - pair[0]).y - Vector3.zero.y), 0f) * Bounce * Time.deltaTime;
	            _velocity += new Vector3((transform.position.x - Vector3.zero.x), (transform.position.y - Vector3.zero.y) / 5, 0f) * Bounce * Time.deltaTime;
	        }
            var mountainTopVertexPairs =
                from vertex1 in _mountainTopVertices
                from vertex2 in _mountainTopVertices
                select new[] { vertex1, vertex2 };
            foreach (var pair in mountainTopVertexPairs)
            {
                if (MyMath.PointLineDistance(pair[0], pair[1], transform.GetComponentInChildren<Circle>().gameObject.transform.position) > 0.25f) continue;
                ResetAcceleration();
                ResetVelocity();
                //	            _velocity += new Vector3(((pair[1] - pair[0]).x - Vector3.zero.x), ((pair[1] - pair[0]).y - Vector3.zero.y), 0f) * Bounce * Time.deltaTime;
                _velocity += new Vector3((transform.position.x - Vector3.zero.x), (transform.position.x - Vector3.zero.x) , 0f) * Bounce * Time.deltaTime;
                Bounce /= 1.005f;
            }

        }
	    if (GroundToCollide != null)
	    {
	        var groundVertexPairs =
	            from vertex1 in _groundVertices
	            from vertex2 in _groundVertices
	            select new[] {vertex1, vertex2};
	        foreach (var pair in groundVertexPairs)
	        {
                if (MyMath.PointLineDistance(pair[0], pair[1], transform.position) > 0.35f) continue;
                ResetAcceleration();
                ResetVelocity();
                _velocity += new Vector3((transform.position.x - Vector3.zero.x)/5, Vector3.up.y * 5, 0f) * Bounce * Time.deltaTime;
	            Bounce /= 1.1f;
	        }
	    }
    }

    private void ResetVelocity()
    {
        _velocity = Vector3.zero;
    }

    private void ResetAcceleration()
    {
        _acceleration = Vector3.zero;
    }

    private void ExtractMountain()
    {
        // Extracting all mountain mesh vertices, for all present parts (Left/Middle/Right) of the mountain
        if (MountainLeft)
        {
            foreach (Vector3 t in MountainToCollide.GetComponentInChildren<MountainLeft>().gameObject.GetComponent<MeshFilter>().mesh.vertices)
            {
                if (!_mountainVertices.Contains(t))
                {
                    _mountainVertices.Add(t);
                }
            }
        }
        if (MountainRight)
        {
            foreach (Vector3 t in MountainToCollide.GetComponentInChildren<MountainRight>().gameObject
                .GetComponent<MeshFilter>()
                .mesh.vertices)
            {
                if (!_mountainVertices.Contains(t))
                {
                    _mountainVertices.Add(t);
                }
            }
        }
    }

    private void ExtractMountainTop()
    {
        if (MountainMiddle)
        {
            foreach (Vector3 t in MountainToCollide.GetComponentInChildren<MountainMiddle>().gameObject
                .GetComponent<MeshFilter>()
                .mesh.vertices)
            {
                if (!_mountainTopVertices.Contains(t))
                {
                    _mountainTopVertices.Add(t);
                }
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
