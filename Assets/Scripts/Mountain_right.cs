using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Mountain_right : MonoBehaviour
{
    private Vector3[] vertices;
    private Vector2[] UV;
    private Vector3[] normals;
    private int[] triangles;
    public int Width;
    public int Height;
    [Range(1, 10)] public int recursionLevels;
    private int vertexCount = 4;
    private int triangleCount;
    [Range(1, 50)] public int roughness;
    // Use this for initialization
    void Start ()
	{
	    GetComponent<MeshFilter>().mesh = GenerateBaseMesh();
	}

    private Mesh GenerateBaseMesh()
    {
        // 3 Base vertices for the main triangle

        // Adding up powers of two
        // On each recursion level, two new vertices are added
        // 1) Midpoint of the side of the triangle
        // 2) Somewhere above the midpoint

        for (int i = 1; i < recursionLevels + 1; i++)
        {
            vertexCount += (int)Mathf.Pow(2.0f, (float)i);
        }
        triangleCount = 3 * (vertexCount - 2);
        Debug.Log(triangleCount);
        // Instantiating the mesh and the arrays needed for it
        Mesh mesh = new Mesh(); 
        vertices = new Vector3[vertexCount];
        UV = new Vector2[vertexCount];
        triangles = new int[triangleCount]; 
        normals = new Vector3[vertexCount];

        // Just assigning UV's because of convention
        // Not really important for this mesh cause of no texture
        for (int i = 0; i < UV.Length; i++)
        {
            UV[i] = new Vector2(0f, 0f);
        }
        // All the normals can point out of screen (-Z axis) ==> 2D game.
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.forward;
        }

        // Base mesh vertices.
        vertices[0] = new Vector3(0f, 0f); // Bottom left vertex
        vertices[1] = new Vector3(Width / 2, 0f); // Bottom right vertex
        vertices[vertexCount - 1] = new Vector3(Width / 6, Height); // Top vertex
        vertices[vertexCount / 2] = new Vector3((vertices[vertexCount - 1].x + vertices[1].x) / 2,
                                                (vertices[vertexCount - 1].y + vertices[1].y) / 2); // Mid point between bottom left & top

        // Setting up left triangle
        // Based on the bottom right, bottom left, and midpoint vertex
        triangles[0] = 0;
        triangles[1] = vertexCount / 2;
        triangles[2] = 1;

        // Setting up right triangle
        // Based on the bottom right, bottom left, and midpoint vertex
        triangles[triangleCount/2] = 0;
        triangles[triangleCount/2 + 1] = vertexCount - 1;
        triangles[triangleCount/2 + 2] = vertexCount / 2;

        // The next recursion level will deal with less vertices and recursion levels than the total.
        int rec_vertexCount = vertexCount - (int) Mathf.Pow(2.0f, (float) recursionLevels);
        GenerateRecursiveMesh(rec_vertexCount, triangleCount/2, 0, 1, recursionLevels - 1);
        GenerateRecursiveMesh(rec_vertexCount, triangleCount/2, triangleCount/2, vertexCount/2, recursionLevels - 1);

        // Linking the generated arrays to the actual Mesh's DS.
        mesh.name = "Mountain";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = UV;
        mesh.normals = normals;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void GenerateRecursiveMesh(int vertexCount, int triangleCount, int currentTriangleIndex, int leftVertexIndex, int recursionLevel)
    {
        // right vertex's index has to be found with consideration of previous recursive level
        int rightVertexIndex = leftVertexIndex + (int) Mathf.Pow(2.0f, (float) (recursionLevel + 1));

        // Calculating the midpoint using linear interpolation
        Vector3 midpoint = Vector3.Lerp(vertices[leftVertexIndex], vertices[rightVertexIndex], 0.5f);
        // Calculating normal using following formula: a = (x_1, y_1), b = (x_2, y_2), dx = x_2 - x_1 , dy = y_2 - y_1
        // normal = (-dy, dx) or (dy, -dx)
        Vector3 normal = new Vector3( -(vertices[rightVertexIndex].y - vertices[leftVertexIndex].y),
            vertices[rightVertexIndex].x - vertices[leftVertexIndex].x);

        float normalDirection = Random.value;
        if (normalDirection < 0.5f)
        {
            normal.Scale(-Vector3.one);
        }
        // Random value between 0 and 1 that determines normal's direction, since we have two normals for each segment
        
        midpoint +=
            normal / (roughness * Vector3.Distance(vertices[leftVertexIndex], vertices[rightVertexIndex]));
        // placing midpoint in vertices array
        vertices[leftVertexIndex + (int) Mathf.Pow(2.0f, (float) recursionLevel)] = midpoint;

        // New left triangle
        triangles[currentTriangleIndex] = 0;
        triangles[currentTriangleIndex + 1] = leftVertexIndex + 1;
        triangles[currentTriangleIndex + 2] = leftVertexIndex;
        
        // New right triangle
        triangles[currentTriangleIndex + 3] = 0;
        triangles[currentTriangleIndex + 4] = leftVertexIndex + 2;
        triangles[currentTriangleIndex + 5] = leftVertexIndex + 1;

        if (recursionLevel != 0)
        {
            int rec_vertexCount = vertexCount - (int) Mathf.Pow(2.0f, (float) recursionLevel);
            GenerateRecursiveMesh(rec_vertexCount, triangleCount/2, currentTriangleIndex, leftVertexIndex, recursionLevel - 1);
            GenerateRecursiveMesh(rec_vertexCount, triangleCount/2, currentTriangleIndex + triangleCount / 2,
                leftVertexIndex + vertexCount / 2 - 1, recursionLevel - 1);
        }
    }
}
