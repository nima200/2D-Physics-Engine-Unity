using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MountainLeft : MonoBehaviour
{
    private Vector3[] _vertices;
    private Vector2[] _uv;
    private Vector3[] _normals;
    private int[] _triangles;
    private float _width;
    private int _height;
    private int _recursionLevels;
    private int _vertexCount = 4;
    private int _triangleCount;
    private int _smoothness;
    // Use this for initialization
    private void Start ()
	{
	    GetComponent<MeshFilter>().mesh = GenerateBaseMesh();
	}

    private Mesh GenerateBaseMesh()
    {
        // Extract global parameters between all mountain parts from the Mountain class.
        Mountain mountain = GameObject.Find("Mountain").GetComponent<Mountain>();
        _width = mountain.Width;
        _height = mountain.Height;
        _recursionLevels = mountain.RecursionLevels;
        _smoothness = mountain.Smoothness;

        // 4 Base vertices for the main triangle

        // Adding up powers of two
        // On each recursion level, two new vertices are added
        // 1) On middle of left triangle
        // 2) On middle of right triangle

        for (int i = 1; i < _recursionLevels + 1; i++)
        {
            _vertexCount += (int)Mathf.Pow(2.0f, (float)i);
        }
        _triangleCount = 3 * (_vertexCount - 2);
        // Instantiating the mesh and the arrays needed for it
        Mesh mesh = new Mesh(); 
        _vertices = new Vector3[_vertexCount];
        _uv = new Vector2[_vertexCount];
        _triangles = new int[_triangleCount]; 
        _normals = new Vector3[_vertexCount];

        // Just assigning UV's because of convention
        // Not really important for this mesh cause of no texture
        for (int i = 0; i < _uv.Length; i++)
        {
            _uv[i] = new Vector2(0f, 0f);
        }
        // All the normals can point out of screen (-Z axis) ==> 2D game.
        for (int i = 0; i < _normals.Length; i++)
        {
            _normals[i] = -Vector3.forward;
        }

        // Base mesh vertices.
        _vertices[0] = new Vector3(0f, 0f); // Bottom right vertex
        _vertices[1] = new Vector3(-_width / 2, 0f); // Bottom left vertex
        _vertices[_vertexCount - 1] = new Vector3(-_width / 6, _height); // Top vertex
        _vertices[_vertexCount / 2] = new Vector3((_vertices[_vertexCount - 1].x + _vertices[1].x) / 2,
                                                (_vertices[_vertexCount - 1].y + _vertices[1].y) / 2); // Mid point between bottom left & top

        // Setting up left triangle
        // Based on the bottom right, bottom left, and midpoint vertex
        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = _vertexCount/2;

        // Setting up right triangle
        // Based on the bottom right, bottom left, and midpoint vertex
        _triangles[_triangleCount/2] = 0;
        _triangles[_triangleCount/2 + 1] = _vertexCount / 2;
        _triangles[_triangleCount/2 + 2] = _vertexCount - 1;

        // The next recursion level will deal with less vertices and recursion levels than the total.
        int recVertexCount = _vertexCount - (int) Mathf.Pow(2.0f, (float) _recursionLevels);
        GenerateRecursiveMesh(recVertexCount, _triangleCount/2, 0, 1, _recursionLevels - 1);
        GenerateRecursiveMesh(recVertexCount, _triangleCount/2, _triangleCount/2, _vertexCount/2, _recursionLevels - 1);

        // Linking the generated arrays to the actual Mesh's DS.
        mesh.name = "Mountain";
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.uv = _uv;
        mesh.normals = _normals;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void GenerateRecursiveMesh(int vertexCount, int triangleCount, int currentTriangleIndex, int leftVertexIndex, int recursionLevel)
    {
        while (true)
        {
            // right vertex's index has to be found with consideration of previous recursive level
            int rightVertexIndex = leftVertexIndex + (int) Mathf.Pow(2.0f, (float) (recursionLevel + 1));

            // Calculating the midpoint using linear interpolation
            Vector3 midpoint = Vector3.Lerp(_vertices[leftVertexIndex], _vertices[rightVertexIndex], 0.5f);
            // Calculating normal using following formula: a = (x_1, y_1), b = (x_2, y_2), dx = x_2 - x_1 , dy = y_2 - y_1
            // normal = (-dy, dx) or (dy, -dx)
            Vector3 normal = new Vector3(-(_vertices[rightVertexIndex].y - _vertices[leftVertexIndex].y), _vertices[rightVertexIndex].x - _vertices[leftVertexIndex].x);
            // Random value between 0 and 1 that determines normal's direction, since we have two normals for each segment
            float normalDirection = Random.value;
            if (normalDirection < 0.5f)
            {
                normal.Scale(-Vector3.one);
            }
            midpoint += normal / (_smoothness * Vector3.Distance(_vertices[leftVertexIndex], _vertices[rightVertexIndex]));
            // placing midpoint in vertices array
            _vertices[leftVertexIndex + (int) Mathf.Pow(2.0f, (float) recursionLevel)] = midpoint;

            // New left triangle
            _triangles[currentTriangleIndex] = 0;
            _triangles[currentTriangleIndex + 1] = leftVertexIndex;
            _triangles[currentTriangleIndex + 2] = leftVertexIndex + 1;

            // New right triangle
            _triangles[currentTriangleIndex + 3] = 0;
            _triangles[currentTriangleIndex + 4] = leftVertexIndex + 1;
            _triangles[currentTriangleIndex + 5] = leftVertexIndex + 2;

            // Stopping the iteration 
            if (recursionLevel == 0) break;

            int recVertexCount = vertexCount - (int)Mathf.Pow(2.0f, (float)recursionLevel);

            GenerateRecursiveMesh(recVertexCount, triangleCount / 2, currentTriangleIndex, leftVertexIndex, recursionLevel - 1);

            // The following attributes simulate a tail recursive call onto the right triangle.
            // Keeping the stack as empty as possible

            int triangleCount1 = triangleCount;
            int vertexCount1 = vertexCount;
            vertexCount = recVertexCount;
            triangleCount = triangleCount / 2;
            currentTriangleIndex = currentTriangleIndex + triangleCount1 / 2;
            leftVertexIndex = leftVertexIndex + vertexCount1 / 2 - 1;
            recursionLevel = recursionLevel - 1;
        }
    }
}
