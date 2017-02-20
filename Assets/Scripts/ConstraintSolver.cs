using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ConstraintSolver
{
    // Dictionary for storing the index of a pair of vertices involved in a constraint as a key, and their distance as the value
    private static readonly Dictionary<Pair<int, int>, float> Dists = new Dictionary<Pair<int, int>, float>();

    public static void Set(int index1, int index2, Vector3 a, Vector3 b)
    {
        if (Dists.ContainsKey(new Pair<int, int>(index1, index2))) return;
        var indices = new Pair<int, int>(index1, index2);
        Dists.Add(indices, Vector3.Distance(a, b));
    }

    private static Pair<Vector3, Vector3> Solve( Vector3 a, Vector3 b, int index1, int index2, float timeStep)
    {
        // Vertlet constraint formula
        // https://gamedevelopment.tutsplus.com/tutorials/simulate-tearable-cloth-and-ragdolls-with-simple-verlet-integration--gamedev-519
        var indices = new Pair<int, int>(index1, index2);
        float restingDistance = Dists[indices];
        var delta = a - b;
        delta.Normalize();
        float currentDistance = (Vector3.Distance(a, b));
        float difference = (restingDistance - currentDistance) / currentDistance;
        a += delta * 0.5f * difference * timeStep;
        b -= delta * 0.5f * difference * timeStep;
        return new Pair<Vector3, Vector3>(a, b);
    }

    public static void Update(List<Verlet> vertletPositions, float timeStep)
    {
        foreach (var pair in Dists)
        { 
            var result = Solve(vertletPositions[pair.Key.AValue].CurrentPosition, vertletPositions[pair.Key.BValue].CurrentPosition, pair.Key.AValue,
                pair.Key.BValue, timeStep);
            vertletPositions[pair.Key.AValue].CurrentPosition = result.AValue;
            vertletPositions[pair.Key.BValue].CurrentPosition = result.BValue;
        }
    }
}