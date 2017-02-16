using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Circle : MonoBehaviour
{

    private Vector3[] _vertices;
    private Vector3[] _normals;
    private Vector2[] _uvs;
    private int[] _triangles;
    public float Radius;
    public int Segments;

    // Use this for initialization
    private void Start()
    {
        GetComponent<MeshFilter>().mesh = GenerateCircle();
    }

    private Mesh GenerateCircle()
    {
        if (Segments < 3) Segments = 3;
        float step = (2 * Mathf.PI) / Segments;
        float tanStep = Mathf.Tan(step);
        float radStep = Mathf.Cos(step);

        Mesh mesh = new Mesh();
        _vertices = new Vector3[Segments + 1];
        _normals = new Vector3[Segments + 1];
        _uvs = new Vector2[Segments + 1];
        _triangles = new int[3 * Segments];
        _vertices[0] = Vector3.zero;
        float x = Radius;
        float y = 0;
        for (int i = 0; i < _normals.Length; i++)
        {
            _normals[i] = Vector3.back;
            _uvs[i] = Vector2.zero;
        }
        for (int i = 0; i < Segments + 1; i++)
        {
            float tx = -y;
            float ty = x;
            x += tx * tanStep;
            y += ty * tanStep;
            x *= radStep;
            y *= radStep;
            _vertices[i] = new Vector3(x, y);
        }
        int index = 1;
        for (int i = 0; i < _triangles.Length; i += 3)
        {
            _triangles[i + 1] = 0;
            _triangles[i] = index;
            if (i >= _triangles.Length - 3)
            {
                _triangles[i + 2] = 1;
            }
            else
            {
                _triangles[i + 2] = index + 1;
            }
            index++;
        }
        mesh.name = "Circle Mesh";
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.normals = _normals;
        mesh.uv = _uvs;

        return mesh;
    }
    
}
