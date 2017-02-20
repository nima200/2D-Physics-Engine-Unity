using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Goat : MonoBehaviour
{
    
    public GameObject VerletPrefab;
    private List<Vector3> _mountainVertices;
    private List<Vector3> _groundVertices;
    private List<Vector3> _verletPositions;
    private List<GameObject> _verletLines;
    private List<GameObject> _myVerlets;
    public float Mass = 1.0f;
    private float _gravity;
    private float _wind;
    private float _angle;
    private float _initialVelocity;
    private float _airResistance;
    private Vector3 _velocity;
    
    public Vector3[] _currentPosition;
    public Vector3[] _lastPosition;
    private Vector3[] _force;

    public Mountain Mountain;
    public Ground Ground;
    public Cannon Cannon;
    public EnvironmentForces Forces;
    public int BondStrength;

    private bool hasCollided = false;

    private void Awake()
    {
        _angle = Cannon.Angle;
        _initialVelocity = Cannon.Velocity;
        _velocity = Vector3.zero;
        _velocity.x = -_initialVelocity * Mathf.Cos(Mathf.Deg2Rad * _angle);
        _velocity.y = _initialVelocity * Mathf.Sin(Mathf.Deg2Rad * _angle);
        
        _wind = Forces.Wind;
        _gravity = Forces.Gravity;
        _airResistance = Forces.AirResistance;
        _verletLines = new List<GameObject>();
        _mountainVertices = new List<Vector3>();
        _groundVertices = new List<Vector3>();
        _myVerlets = new List<GameObject>();
//        for (int i = 0; i < 25; i++)
//        {
//            var line = new GameObject("Line " + (i + 1));
//            line.transform.parent = gameObject.transform;
//            var lr = line.AddComponent<LineRenderer>();
//            lr.material.color = Color.black;
//            lr.startWidth = lr.endWidth = 0.1f;
//            lr.numPositions = 2;
//            _verletLines.Add(line);
//        }
        _verletPositions = new List<Vector3>
        {
            // body
//            Cannon.transform.position + new Vector3(-0.4f, 0.25f), // 0
//            Cannon.transform.position + new Vector3(-0.3f, 0.25f), // 1
//            Cannon.transform.position + new Vector3(-0.2f, 0.2f), // 2
//            Cannon.transform.position + new Vector3(0.35f, 0.15f), // 3
//            Cannon.transform.position + new Vector3(0.4f, 0.1f), // 4
//            Cannon.transform.position + new Vector3(0.35f, -0.05f), // 5
//            Cannon.transform.position + new Vector3(0.25f, -0.1f), // 6
//            Cannon.transform.position + new Vector3(0f, 0f), // 7
//            Cannon.transform.position + new Vector3(-0.20f, 0f), // 8
//            Cannon.transform.position + new Vector3(-0.35f, 0.1f), // 9
//            Cannon.transform.position + new Vector3(-0.4f, 0.05f), // 10
//            Cannon.transform.position + new Vector3(-0.45f, 0.05f), // 11 
//            Cannon.transform.position + new Vector3(-0.5f, 0.0f), // 12
//            Cannon.transform.position + new Vector3(-0.5f, 0.1f), // 13
//            // horn
//            Cannon.transform.position + new Vector3(-0.35f, 0.35f), // 14
//            Cannon.transform.position + new Vector3(-0.35f, 0.25f), // 15
//            // tail
//            Cannon.transform.position + new Vector3(0.5f, 0.25f), // 16
//            // front leg
//            Cannon.transform.position + new Vector3(0f, -0.2f), // 17
//            Cannon.transform.position + new Vector3(0f, -0.35f), // 18
//            // back leg
//            Cannon.transform.position + new Vector3(0.25f, -0.2f), // 19
//            Cannon.transform.position + new Vector3(0.25f, -0.35f), // 20
//            // beard
//            Cannon.transform.position + new Vector3(-0.4f, -0.05f), // 21
//            // eye
//            Cannon.transform.position + new Vector3(-0.4f, 0.2f) // 22

            gameObject.transform.position + new Vector3(-0.1f, -0.1f),
            gameObject.transform.position + new Vector3(-0.1f, 0.1f),
            gameObject.transform.position + new Vector3(0.1f, 0.1f),
            gameObject.transform.position + new Vector3(0.1f, -0.1f),

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
        _force = new Vector3[_verletPositions.Count];

        for (int i = 0; i < _verletPositions.Count; i++)
        {
            _currentPosition[i] = _verletPositions[i];
            _lastPosition[i] = _verletPositions[i] - _velocity * Time.deltaTime; 
            _force[i] = Vector3.zero;
        }

        ConstraintSolver.Set(0, 1, _currentPosition[0], _currentPosition[1]);
        ConstraintSolver.Set(1, 0, _currentPosition[1], _currentPosition[0]);
        ConstraintSolver.Set(1, 2, _currentPosition[1], _currentPosition[2]);
        ConstraintSolver.Set(2, 1, _currentPosition[2], _currentPosition[1]);
        ConstraintSolver.Set(2, 3, _currentPosition[2], _currentPosition[3]);
        ConstraintSolver.Set(3, 2, _currentPosition[3], _currentPosition[2]);
        ConstraintSolver.Set(3, 0, _currentPosition[3], _currentPosition[0]);
        ConstraintSolver.Set(0, 3, _currentPosition[0], _currentPosition[3]);
        ConstraintSolver.Set(3, 1, _currentPosition[3], _currentPosition[1]);
        ConstraintSolver.Set(1, 3, _currentPosition[1], _currentPosition[3]);
        ConstraintSolver.Set(0, 2, _currentPosition[0], _currentPosition[2]);
        ConstraintSolver.Set(2, 0, _currentPosition[2], _currentPosition[0]);
        
//        ConstraintSolver.Set(0, 13, _currentPosition[0], _currentPosition[13]);
//        ConstraintSolver.Set(1, 15, _currentPosition[1], _currentPosition[15]);
//        ConstraintSolver.Set(1, 2, _currentPosition[1], _currentPosition[2]);
//        ConstraintSolver.Set(2, 3, _currentPosition[2], _currentPosition[3]);
//        ConstraintSolver.Set(3, 16, _currentPosition[3], _currentPosition[16]);
//        ConstraintSolver.Set(3, 4, _currentPosition[3], _currentPosition[4]);
//        ConstraintSolver.Set(4, 16, _currentPosition[4], _currentPosition[16]);
//        ConstraintSolver.Set(4, 5, _currentPosition[4], _currentPosition[5]);
//        ConstraintSolver.Set(5, 6, _currentPosition[5], _currentPosition[6]);
//        ConstraintSolver.Set(6, 19, _currentPosition[6], _currentPosition[19]);
//        ConstraintSolver.Set(6, 7, _currentPosition[6], _currentPosition[7]);
//        ConstraintSolver.Set(7, 17, _currentPosition[7], _currentPosition[17]);
//        ConstraintSolver.Set(7, 8, _currentPosition[7], _currentPosition[8]);
//        ConstraintSolver.Set(8, 9, _currentPosition[8], _currentPosition[9]);
//        ConstraintSolver.Set(9, 10, _currentPosition[9], _currentPosition[10]);
//        ConstraintSolver.Set(10, 11, _currentPosition[10], _currentPosition[11]);
//        ConstraintSolver.Set(10, 21, _currentPosition[10], _currentPosition[21]);
//        ConstraintSolver.Set(11, 21, _currentPosition[11], _currentPosition[21]);
//        ConstraintSolver.Set(11, 12, _currentPosition[11], _currentPosition[12]);
//        ConstraintSolver.Set(12, 13, _currentPosition[12], _currentPosition[13]);
//
//        ConstraintSolver.Set(17, 18, _currentPosition[17], _currentPosition[18]);
//        ConstraintSolver.Set(19, 20, _currentPosition[19], _currentPosition[20]);
//        ConstraintSolver.Set(0, 15, _currentPosition[0], _currentPosition[15]);
//        ConstraintSolver.Set(0, 14, _currentPosition[0], _currentPosition[14]);
//        ConstraintSolver.Set(14, 15, _currentPosition[14], _currentPosition[15]);
//                ConstraintSolver.Update(_currentPosition);
    }

    private void Start()
    {
        ExtractMeshVertices();
        ExtractGroundVertices();
    }

    private void Update()
    {
       
        // Update environment forces on a frame basis
        for (int i = 0; i < _force.Length; i++)
        {
            _force[i].y -= Mass * (_gravity) * Time.deltaTime;
            _force[i] += _velocity * _wind * Time.deltaTime;
            _force[i] -= _velocity * _airResistance * Time.deltaTime;
            
        }
        // Update Ragdoll position on a frame basis
        for (int i = 0; i < _currentPosition.Length; i++)
        {
//            if (!hasCollided)
//            {
                var pos = _currentPosition[i];
                _currentPosition[i] =
                    new Vector3(
                        _currentPosition[i].x + _currentPosition[i].x - _lastPosition[i].x +
                        _force[i].x * Mathf.Pow(Time.deltaTime, 2),
                        _currentPosition[i].y + _currentPosition[i].y - _lastPosition[i].y +
                        _force[i].y * Mathf.Pow(Time.deltaTime, 2));
                _lastPosition[i] = pos;
                _myVerlets[i].transform.position = _currentPosition[i];
//            }
        }
        // Collision Detection
        Detect();
        
        /*Vector3 delta = _currentPosition[1] - _currentPosition[0];
        float deltaLength = Mathf.Sqrt(Vector3.Dot(delta, delta));
        float diff = (deltaLength - Vector3.Distance(_currentPosition[1], _currentPosition[0]));
        _currentPosition[0] = _currentPosition[0] + delta * 0.5f * diff;
        _currentPosition[1] = _currentPosition[1] - delta * 0.5f * diff;

        delta = _currentPosition[2] - _currentPosition[1];
        deltaLength = Mathf.Sqrt(Vector3.Dot(delta, delta));
        diff = (deltaLength - Vector3.Distance(_currentPosition[2], _currentPosition[1]));
        _currentPosition[1] = _currentPosition[1] + delta * 0.5f * diff;
        _currentPosition[2] = _currentPosition[2] - delta * 0.5f * diff;

        delta = _currentPosition[3] - _currentPosition[2];
        deltaLength = Mathf.Sqrt(Vector3.Dot(delta, delta));
        diff = (deltaLength - Vector3.Distance(_currentPosition[3], _currentPosition[2]));
        _currentPosition[2] = _currentPosition[2] + delta * 0.5f * diff;
        _currentPosition[3] = _currentPosition[3] - delta * 0.5f * diff;

        delta = _currentPosition[0] - _currentPosition[3];
        deltaLength = Mathf.Sqrt(Vector3.Dot(delta, delta));
        diff = (deltaLength - Vector3.Distance(_currentPosition[0], _currentPosition[3]));
        _currentPosition[3] = _currentPosition[3] + delta * 0.5f * diff;
        _currentPosition[0] = _currentPosition[0] - delta * 0.5f * diff;*/



//        for (int j = 0; j < BondStrength; j++)
//        {
//            ConstraintSolver.Update(_currentPosition);
//        }
    }

    private void Detect()
    {
        for (int i = 0; i < _currentPosition.Length; i++)
        {
            for (int j = 0; j < _mountainVertices.Count - 1; j++)
            {
                var a = _mountainVertices[j];
                var b = _mountainVertices[j + 1];
                if (MyMath.PointLineDistance(a, b, _currentPosition[i]) < 0.1f)
                {

                    _currentPosition[i] = _lastPosition[i];
                    _lastPosition[i] =_currentPosition[i];
                    
//                    hasCollided = true;
//                    return;
                }
            }
            for (int j = 0; j < _groundVertices.Count - 1; j++)
            {
                var a = _groundVertices[j];
                var b = _groundVertices[j + 1];
                if (MyMath.PointLineDistance(a, b, _currentPosition[i]) < 0.1f)
                {
                    _currentPosition[i] = _lastPosition[i];
                    _lastPosition[i] = _currentPosition[i];
//                    hasCollided = true;
//                    return;
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