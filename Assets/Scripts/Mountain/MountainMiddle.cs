using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MountainMiddle : MonoBehaviour {
    private List<Vector3> _vertices;
    private Vector3[] _verticesArray;
    private List<int> _triangles;
    private int[] _trianglesArray;
    private Vector2[] _uVs;
    private Vector3[] _normals;
    private float _width;
    private int _height;
	// Use this for initialization
    private void Start () {
        GetComponent<MeshFilter>().mesh = GenerateBaseMesh();
    }

    private Mesh GenerateBaseMesh()
    {
        var mesh = new Mesh();
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        // Extract global parameters between all mountain parts from the Mountain class.
        var mountain = GameObject.Find("Mountain").GetComponent<Mountain>();
        _width = mountain.Width;
        _height = mountain.Height;

        _vertices.Add(new Vector3(0f, 0f));
        float increment = _width / 3 / 32;
        for (float i = -_width / 6; i < _width / 6; i+= increment)
        {
            _vertices.Add(new Vector3(i, _height));
        }
        _vertices.Add(new Vector3(_width / 6, _height));
        _verticesArray = _vertices.ToArray();
        for (int i = 1; i < _verticesArray.Length - 1; i++)
        {
            _triangles.Add(0);
            _triangles.Add(i);
            _triangles.Add(i+1);
        }
        _trianglesArray = _triangles.ToArray();
        _uVs = new Vector2[_verticesArray.Length];
        _normals = new Vector3[_verticesArray.Length];
        for (int i = 0; i < _verticesArray.Length; i++)
        {
            _uVs[i] = Vector2.zero;
            _normals[i] = Vector3.back;
        }
        mesh.name = "Mountain_Middle";
        mesh.vertices = _verticesArray;
        mesh.triangles = _trianglesArray;
        mesh.normals = _normals;
        mesh.uv = _uVs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
