using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Range(0, 40)] public int Velocity;
    [Range(0, 80)] public int Angle;
    public PhysicsEngine_2D ProjectilePrefab;
    public ProjectileType MyProjectileType;

    private void Awake()
    {
        MyProjectileType = ProjectilePrefab.ProjectileType;
        gameObject.name = MyProjectileType == ProjectileType.Ball ? "BallCannon" : "GoatCannon";
    }

	// Update is called once per frame
    private void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Instantiate(ProjectilePrefab, gameObject.GetComponentInChildren<Barrel>().gameObject.transform, false);
	    }
	}
}