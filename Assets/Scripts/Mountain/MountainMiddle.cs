using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MountainMiddle : MonoBehaviour {
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uVs;
    private Vector3[] _normals;
    private float _width;
    private int _height;
	// Use this for initialization
	void Start () {
        GetComponent<MeshFilter>().mesh = GenerateBaseMesh();
    }
	
	Mesh GenerateBaseMesh()
    {
        Mesh mesh = new Mesh();
        _vertices = new Vector3[3];
        _triangles = new int[3];
        _uVs = new Vector2[3];
        _normals = new Vector3[3];
        // Extract global parameters between all mountain parts from the Mountain class.
        Mountain mountain = GameObject.Find("Mountain").GetComponent<Mountain>();
        _width = mountain.Width;
        _height = mountain.Height;

        _vertices[0] = new Vector3(0f, 0f);
        _vertices[1] = new Vector3(-_width / 6, _height);
        _vertices[2] = new Vector3(_width / 6, _height);
        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = 2;
        _uVs[0] = _uVs[1] = _uVs[2] = new Vector2(0f, 0f);
        _normals[0] = _normals[1] = _normals[2] = -Vector3.forward;
        mesh.name = "Mountain_Middle";
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.normals = _normals;
        mesh.uv = _uVs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
