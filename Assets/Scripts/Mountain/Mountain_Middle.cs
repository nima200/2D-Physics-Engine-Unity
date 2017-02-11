using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Mountain_Middle : MonoBehaviour {
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] UVs;
    private Vector3[] normals;
    private int Width;
    private int Height;
	// Use this for initialization
	void Start () {
        GetComponent<MeshFilter>().mesh = GenerateBaseMesh();
    }
	
	Mesh GenerateBaseMesh()
    {
        Mesh mesh = new Mesh();
        vertices = new Vector3[3];
        triangles = new int[3];
        UVs = new Vector2[3];
        normals = new Vector3[3];
        // Extract global parameters between all mountain parts from the Mountain class.
        Mountain mountain = GameObject.Find("Mountain").GetComponent<Mountain>();
        Width = mountain.Width;
        Height = mountain.Height;

        vertices[0] = new Vector3(0f, 0f);
        vertices[1] = new Vector3(-Width / 6, Height);
        vertices[2] = new Vector3(Width / 6, Height);
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        UVs[0] = UVs[1] = UVs[2] = new Vector2(0f, 0f);
        normals[0] = normals[1] = normals[2] = -Vector3.forward;
        mesh.name = "Mountain_Middle";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
