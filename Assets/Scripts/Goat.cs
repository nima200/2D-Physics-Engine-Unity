using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Goat : MonoBehaviour
{
    
    public GameObject VerletPrefab;
    public List<Vector3> _mountainVertices;
    private List<Vector3> _groundVertices;
    private List<Vector3> _verletPositions;
    private List<GameObject> _verletLines;
    public List<GameObject> _myVerlets;

    public float Mass = 1.0f;
    public float _gravity;
    public float _wind;
    public float _angle;
    public float _initialVelocity;
    public float _airResistance;
    public Vector3 _velocity;
    public Vector3 _acceleration;

    public Vector3[] _currentPosition;
    private Vector3[] _lastPosition;
    private Vector3[] _forceAccumulation;

    public Mountain Mountain;
    public Ground Ground;
    public Cannon Cannon;
    public EnvironmentForces Forces;
    

    private void Awake()
    {
        _angle = Cannon.Angle;
        _initialVelocity = Cannon.Velocity;
        _velocity = Vector3.zero;
        _acceleration = Vector3.zero;
        _velocity.x = -_initialVelocity * Mathf.Cos(Mathf.Deg2Rad * _angle);
        _velocity.y = _initialVelocity * Mathf.Sin(Mathf.Deg2Rad * _angle);
        
        _wind = Forces.Wind;
        _gravity = Forces.Gravity;
        _airResistance = Forces.AirResistance;
        _verletLines = new List<GameObject>();
        _mountainVertices = new List<Vector3>();
        _groundVertices = new List<Vector3>();
        _myVerlets = new List<GameObject>();
        for (int i = 0; i < 25; i++)
        {
            var line = new GameObject("Line " + (i + 1));
            line.transform.parent = gameObject.transform;
            var lr = line.AddComponent<LineRenderer>();
            lr.material.color = Color.black;
            lr.startWidth = lr.endWidth = 0.1f;
            lr.numPositions = 2;
            _verletLines.Add(line);
        }
        _verletPositions = new List<Vector3>
        {
            // body
            Cannon.transform.position + new Vector3(-0.4f, 0.25f),
            Cannon.transform.position + new Vector3(-0.3f, 0.25f),
            Cannon.transform.position + new Vector3(-0.2f, 0.2f),
            Cannon.transform.position + new Vector3(0.35f, 0.15f),
            Cannon.transform.position + new Vector3(0.4f, 0.1f),
            Cannon.transform.position + new Vector3(0.35f, -0.05f),
            Cannon.transform.position + new Vector3(0.25f, -0.1f),
            Cannon.transform.position + new Vector3(0f, 0f),
            Cannon.transform.position + new Vector3(-0.15f, 0f),
            Cannon.transform.position + new Vector3(-0.35f, 0.1f),
            Cannon.transform.position + new Vector3(-0.4f, 0.05f),
            Cannon.transform.position + new Vector3(-0.45f, 0.05f),
            Cannon.transform.position + new Vector3(-0.5f, 0.0f),
            Cannon.transform.position + new Vector3(-0.5f, 0.1f),
            // horn
            Cannon.transform.position + new Vector3(-0.35f, 0.35f),
            Cannon.transform.position + new Vector3(-0.35f, 0.25f),
            // tail
            Cannon.transform.position + new Vector3(0.5f, 0.25f),
            // front leg
            Cannon.transform.position + new Vector3(0f, -0.2f),
            Cannon.transform.position + new Vector3(0f, -0.35f),
            // back leg
            Cannon.transform.position + new Vector3(0.25f, -0.2f),
            Cannon.transform.position + new Vector3(0.25f, -0.35f),
            // beard
            Cannon.transform.position + new Vector3(-0.4f, -0.05f),
            // eye
            Cannon.transform.position + new Vector3(-0.4f, 0.2f),

        };

        foreach (var t in _verletPositions)
        {
            var verlet = Instantiate(VerletPrefab);
            verlet.transform.SetParent(transform, false);
            verlet.transform.position = t;
            _myVerlets.Add(verlet);
        }
        


        _currentPosition = new Vector3[_verletPositions.Count];
        _lastPosition = new Vector3[_verletPositions.Count];
        _forceAccumulation = new Vector3[_verletPositions.Count];

        for (int i = 0; i < _verletPositions.Count; i++)
        {
            _currentPosition[i] = _verletPositions[i];
            _lastPosition[i] = _verletPositions[i] - _velocity * Time.deltaTime;
            _forceAccumulation[i] = Vector3.zero;
        }

        
    }

    private void Start()
    {
        ExtractMeshVertices();
        ExtractGroundVertices();
    }

    private void Update()
    {
        // Update environment forces on a frame basis
        for (int i = 0; i < _forceAccumulation.Length; i++)
        {
            _forceAccumulation[i].y -= Mass * (_gravity) * Time.deltaTime;
            _forceAccumulation[i] += _velocity * _wind/2 * Time.deltaTime;
            _forceAccumulation[i] -= _velocity * _airResistance * Time.deltaTime;
        }
        // Update Ragdoll position on a frame basis
        for (int i = 0; i < _currentPosition.Length; i++)
        {
            var pos = _currentPosition[i];
            _currentPosition[i] =
                new Vector3(
                    _currentPosition[i].x + _currentPosition[i].x - _lastPosition[i].x +
                    _forceAccumulation[i].x * Mathf.Pow(Time.deltaTime, 2),
                    _currentPosition[i].y + _currentPosition[i].y - _lastPosition[i].y +
                    _forceAccumulation[i].y * Mathf.Pow(Time.deltaTime, 2));
            _lastPosition[i] = pos;
            _myVerlets[i].transform.position = _currentPosition[i];
            
            // Collision Detection
            for (int j = 0; j < _mountainVertices.Count - 1; j++)
            {
                var a = _mountainVertices[j];
                var b = _mountainVertices[j + 1];
                if (MyMath.PointLineDistance(a, b, _currentPosition[i]) < 0.1f)
                {
                    _currentPosition[i] = _lastPosition[i];
                }
            }
            for (int j = 0; j < _groundVertices.Count - 1; j++) 
            {
                var a = _groundVertices[j];
                var b = _groundVertices[j + 1];
                if (MyMath.PointLineDistance(a, b, _currentPosition[i]) < 0.1f)
                {
                    _currentPosition[i] = _lastPosition[i];
                }
            }
        }
    }

    private void ExtractMeshVertices()
    {
        Vector3[] mountLeftVertices =
            Mountain.GetComponentInChildren<MountainLeft>().gameObject.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] mountRightVertices =
            Mountain.GetComponentInChildren<MountainRight>().gameObject.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] mountMidVertices =
            Mountain.GetComponentInChildren<MountainMiddle>().gameObject.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] groundVertices =
            Ground.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        foreach (Vector3 t in mountLeftVertices)
        {
            _mountainVertices.Add(t);
        }
        foreach (Vector3 t in mountRightVertices)
        {
            _mountainVertices.Add(t);
        }
        foreach (Vector3 t in mountMidVertices)
        {
            _mountainVertices.Add(t);
        }
    }

    private void ExtractGroundVertices()
    {
        Vector3[] groundVertices = Ground.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        foreach (Vector3 vertex in groundVertices)
        {
            _groundVertices.Add(vertex);
        }
    }

}