using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileManager
{
    public static readonly List<PhysicsEngine_2D> Projectiles = new List<PhysicsEngine_2D>();

    public static void Add(PhysicsEngine_2D projectile)
    {
        Projectiles.Add(projectile);
    }

    public static void Remove(PhysicsEngine_2D projectile)
    {
        Projectiles.Remove(projectile);
    }
}
