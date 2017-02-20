using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Ground : MonoBehaviour { 

    public int Width;
    public int Height;
    private Vector3[] _vertices;
    private Vector3[] _normals;
    private Vector2[] _uvs;
    private int[] _triangles;

    private void Start ()
    {
        GetComponent<MeshFilter>().mesh = GenerateGroundMesh();
    }

    private Mesh GenerateGroundMesh()
    {
        Mesh mesh = new Mesh {name = "Ground Mesh"};
        _vertices = new Vector3[4];
        _normals = new Vector3[4];
        _uvs = new Vector2[4];
        _triangles = new int[3 * (4 - 2)];

        for (int i = 0; i < 4; i++)
        {
            _normals[i] = Vector3.back;
            _uvs[i] = Vector2.zero;
        }

        _vertices[0] = new Vector3((float) -Width / 2, 0f);
        _vertices[1] = new Vector3((float) Width / 2, 0f);
        _vertices[2] = new Vector3((float) -Width / 2, -Height);
        _vertices[3] = new Vector3((float) Width / 2, -Height);

        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = 2;

        _triangles[3] = 2;
        _triangles[4] = 1;
        _triangles[5] = 3;

        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.normals = _normals;
        mesh.uv = _uvs;

        return mesh;
    }

}
