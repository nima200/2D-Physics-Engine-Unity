using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Animal : MonoBehaviour
{
    private float _airResistance;
    private float _angle;
    private float _gravity;
    private Ground _ground;
    private List<Vector3> _groundVertices;
    private float _initialVelocity;
    private List<LineRenderer> _lines;
    private Mountain _mountain;
    private List<Vector3> _mountainVertices;
    private List<Verlet> _myVerlets;
    private float _wind;
    private int _x;
    private Cannon _cannon;
    private EnvironmentForces _forces;
    public Verlet VerletPrefab;

    private void Awake()
    {
        // Assigning all the gameobjects that our goat needs to interact with.
        _mountain = GameObject.Find("Mountain").GetComponent<Mountain>();
        _ground = GameObject.Find("GroundMesh").GetComponent<Ground>();
        _cannon = GameObject.Find("GoatCannon").GetComponent<Cannon>();
        _forces = GameObject.Find("Environment Forces").GetComponent<EnvironmentForces>();

        // Extracting the attributes needed for physics.
        _angle = _cannon.Angle;
        _initialVelocity = _cannon.Velocity;
        _gravity = _forces.Gravity;
        _airResistance = _forces.AirResistance;
        _myVerlets = new List<Verlet>();
        _mountainVertices = new List<Vector3>();
        _groundVertices = new List<Vector3>();
        _lines = new List<LineRenderer>();

        CreateVerlets();
        // Initialize the forces acting upon the verlet(s) to be 0 at start.
        foreach (var verlet in _myVerlets)
        {
            verlet.CurrentForce = Vector3.zero;
        }
    }

    private void Start()
    {
        // Extract the vertices from the meshes we want to check for collision with
        ExtractMeshVertices();
        ExtractGroundVertices();

        // 21 lines for the goat. Could have probably reduced this by a lot by adding more vertices within the line.
        for (int i = 0; i < 21; i++)
        {
            var line = new GameObject();
            line.transform.SetParent(transform, false);
            var lineR = line.AddComponent<LineRenderer>();
            lineR.material.color = Color.black;
            lineR.startWidth = 0.03f;
            lineR.endWidth = 0.03f;
            lineR.numPositions = 2;
            _lines.Add(lineR);
        }
    }

    private void CreateVerlets()
    {
        // Projectile velocity
        // vi_x = vi * Cos(theta)
        // vi_y = vi * Sin(theta)
        var velocity = new Vector3(-_initialVelocity * Mathf.Cos(Mathf.Deg2Rad * _angle),
            _initialVelocity * Mathf.Sin(Mathf.Deg2Rad * _angle));

        int index = 0;
        // Hardcoding wall - We will make programming great again.
        // * 2f cause I just wanted to scale my goat a bit bigger but i was lazy to recalculate every location
        var v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.3f, 0.15f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.25f, 0.15f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.2f, 0.15f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.15f, 0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.1f, 0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(0.25f, 0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(0.25f, -0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(0.15f, -0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(0f, -0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.1f, -0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.1f, 0f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.15f, 0f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.25f, 0.05f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.3f, 0.05f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.35f, 0f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.35f, 0.05f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.25f, 0.1f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(-0.2f, 0.25f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(0f, -0.25f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        v = Instantiate(VerletPrefab, transform);
        v.InitialPosition = new Vector3(0.15f, -0.25f) * 2f;
        v.Index = index++;
        _myVerlets.Add(v);

        // Assign velocities only to the head area of the goat.
        Shoot(velocity);

        foreach (var verlet in _myVerlets)
        {
            // Assign a fixed timestep variable to all verlets so that 
            // their movement is not affected by frame time
            verlet.TimeStep = Time.fixedDeltaTime;
            verlet.SetPreviousPosition();
        }
        
    }

    private void Shoot(Vector3 velocity)
    {
        _myVerlets[0].SetVelocity(velocity);
        _myVerlets[1].SetVelocity(velocity);
        _myVerlets[2].SetVelocity(velocity);
        _myVerlets[3].SetVelocity(velocity);
        _myVerlets[11].SetVelocity(velocity);
        _myVerlets[12].SetVelocity(velocity);
        _myVerlets[13].SetVelocity(velocity);
        _myVerlets[16].SetVelocity(velocity);
        _myVerlets[15].SetVelocity(velocity);
        _myVerlets[14].SetVelocity(velocity);
        _myVerlets[17].SetVelocity(velocity);
    }

    private void SetConsraints()
    {
        if (_x != 0) return;
        _x++;
        _myVerlets[0].SetConstraint(_myVerlets[1]);
        _myVerlets[0].SetConstraint(_myVerlets[15]);
        _myVerlets[0].SetConstraint(_myVerlets[17]);
        _myVerlets[0].SetConstraint(_myVerlets[16]);
        _myVerlets[0].SetConstraint(_myVerlets[19]);
        _myVerlets[0].SetConstraint(_myVerlets[18]);

        _myVerlets[1].SetConstraint(_myVerlets[17]);
        _myVerlets[1].SetConstraint(_myVerlets[16]);
        _myVerlets[1].SetConstraint(_myVerlets[2]);

        _myVerlets[2].SetConstraint(_myVerlets[1]);
        _myVerlets[2].SetConstraint(_myVerlets[3]);
        _myVerlets[2].SetConstraint(_myVerlets[12]);
        _myVerlets[2].SetConstraint(_myVerlets[11]);
        _myVerlets[2].SetConstraint(_myVerlets[17]);
        _myVerlets[2].SetConstraint(_myVerlets[19]);

        _myVerlets[3].SetConstraint(_myVerlets[4]);
        _myVerlets[3].SetConstraint(_myVerlets[11]);
        _myVerlets[3].SetConstraint(_myVerlets[10]);
        _myVerlets[3].SetConstraint(_myVerlets[12]);
        _myVerlets[3].SetConstraint(_myVerlets[5]);
        _myVerlets[3].SetConstraint(_myVerlets[6]);
        _myVerlets[3].SetConstraint(_myVerlets[7]);
        _myVerlets[3].SetConstraint(_myVerlets[8]);
        _myVerlets[3].SetConstraint(_myVerlets[9]);
        _myVerlets[3].SetConstraint(_myVerlets[13]);
        _myVerlets[3].SetConstraint(_myVerlets[14]);
        _myVerlets[3].SetConstraint(_myVerlets[15]);
        _myVerlets[3].SetConstraint(_myVerlets[16]);

        _myVerlets[4].SetConstraint(_myVerlets[11]);
        _myVerlets[4].SetConstraint(_myVerlets[10]);
        _myVerlets[4].SetConstraint(_myVerlets[8]);
        _myVerlets[4].SetConstraint(_myVerlets[5]);
        _myVerlets[4].SetConstraint(_myVerlets[6]);
        _myVerlets[4].SetConstraint(_myVerlets[18]);
        _myVerlets[4].SetConstraint(_myVerlets[19]);
        _myVerlets[4].SetConstraint(_myVerlets[14]);
        _myVerlets[4].SetConstraint(_myVerlets[9]);
        _myVerlets[4].SetConstraint(_myVerlets[16]);
        _myVerlets[4].SetConstraint(_myVerlets[0]);
        _myVerlets[4].SetConstraint(_myVerlets[1]);
        _myVerlets[4].SetConstraint(_myVerlets[13]);
        _myVerlets[4].SetConstraint(_myVerlets[12]);


        _myVerlets[5].SetConstraint(_myVerlets[9]);
        _myVerlets[5].SetConstraint(_myVerlets[8]);
        _myVerlets[5].SetConstraint(_myVerlets[7]);
        _myVerlets[5].SetConstraint(_myVerlets[6]);
        _myVerlets[5].SetConstraint(_myVerlets[18]);
        _myVerlets[5].SetConstraint(_myVerlets[19]);

        _myVerlets[6].SetConstraint(_myVerlets[7]);
        _myVerlets[6].SetConstraint(_myVerlets[8]);
        _myVerlets[6].SetConstraint(_myVerlets[19]);
        _myVerlets[6].SetConstraint(_myVerlets[5]);

        _myVerlets[7].SetConstraint(_myVerlets[19]);
        _myVerlets[7].SetConstraint(_myVerlets[6]);
        _myVerlets[7].SetConstraint(_myVerlets[8]);
        _myVerlets[7].SetConstraint(_myVerlets[18]);

        _myVerlets[8].SetConstraint(_myVerlets[7]);
        _myVerlets[8].SetConstraint(_myVerlets[19]);
        _myVerlets[8].SetConstraint(_myVerlets[18]);
        _myVerlets[8].SetConstraint(_myVerlets[9]);
        _myVerlets[8].SetConstraint(_myVerlets[10]);

        _myVerlets[9].SetConstraint(_myVerlets[8]);
        _myVerlets[9].SetConstraint(_myVerlets[18]);
        _myVerlets[9].SetConstraint(_myVerlets[10]);
        _myVerlets[9].SetConstraint(_myVerlets[11]);
        _myVerlets[9].SetConstraint(_myVerlets[5]);

        _myVerlets[10].SetConstraint(_myVerlets[9]);
        _myVerlets[10].SetConstraint(_myVerlets[11]);
        _myVerlets[10].SetConstraint(_myVerlets[12]);
        _myVerlets[10].SetConstraint(_myVerlets[4]);
        _myVerlets[10].SetConstraint(_myVerlets[3]);

        _myVerlets[11].SetConstraint(_myVerlets[3]);
        _myVerlets[11].SetConstraint(_myVerlets[4]);
        _myVerlets[11].SetConstraint(_myVerlets[12]);
        _myVerlets[11].SetConstraint(_myVerlets[2]);
        _myVerlets[11].SetConstraint(_myVerlets[18]);
        _myVerlets[11].SetConstraint(_myVerlets[19]);
        _myVerlets[11].SetConstraint(_myVerlets[14]);
        _myVerlets[11].SetConstraint(_myVerlets[5]);
        _myVerlets[11].SetConstraint(_myVerlets[0]);

        _myVerlets[12].SetConstraint(_myVerlets[16]);
        _myVerlets[12].SetConstraint(_myVerlets[13]);
        _myVerlets[12].SetConstraint(_myVerlets[10]);
        _myVerlets[12].SetConstraint(_myVerlets[9]);
        _myVerlets[12].SetConstraint(_myVerlets[11]);
        _myVerlets[12].SetConstraint(_myVerlets[14]);
        _myVerlets[12].SetConstraint(_myVerlets[2]);
        _myVerlets[12].SetConstraint(_myVerlets[0]);
        _myVerlets[12].SetConstraint(_myVerlets[18]);
        _myVerlets[12].SetConstraint(_myVerlets[5]);

        _myVerlets[13].SetConstraint(_myVerlets[12]);
        _myVerlets[13].SetConstraint(_myVerlets[14]);
        _myVerlets[13].SetConstraint(_myVerlets[15]);
        _myVerlets[13].SetConstraint(_myVerlets[0]);
        _myVerlets[13].SetConstraint(_myVerlets[1]);

        _myVerlets[14].SetConstraint(_myVerlets[13]);
        _myVerlets[14].SetConstraint(_myVerlets[15]);
        _myVerlets[14].SetConstraint(_myVerlets[16]);
        _myVerlets[14].SetConstraint(_myVerlets[0]);
        _myVerlets[14].SetConstraint(_myVerlets[5]);
        _myVerlets[14].SetConstraint(_myVerlets[19]);

        _myVerlets[15].SetConstraint(_myVerlets[13]);
        _myVerlets[15].SetConstraint(_myVerlets[14]);
        _myVerlets[15].SetConstraint(_myVerlets[16]);
        _myVerlets[15].SetConstraint(_myVerlets[0]);
        _myVerlets[15].SetConstraint(_myVerlets[1]);
        _myVerlets[15].SetConstraint(_myVerlets[5]);

        _myVerlets[16].SetConstraint(_myVerlets[0]);
        _myVerlets[16].SetConstraint(_myVerlets[1]);
        _myVerlets[16].SetConstraint(_myVerlets[2]);
        _myVerlets[16].SetConstraint(_myVerlets[3]);
        _myVerlets[16].SetConstraint(_myVerlets[12]);
        _myVerlets[16].SetConstraint(_myVerlets[13]);
        _myVerlets[16].SetConstraint(_myVerlets[15]);
        _myVerlets[16].SetConstraint(_myVerlets[14]);

        _myVerlets[17].SetConstraint(_myVerlets[1]);
        _myVerlets[17].SetConstraint(_myVerlets[2]);
        _myVerlets[17].SetConstraint(_myVerlets[0]);
        _myVerlets[17].SetConstraint(_myVerlets[3]);
        _myVerlets[17].SetConstraint(_myVerlets[4]);
        _myVerlets[17].SetConstraint(_myVerlets[5]);
        _myVerlets[17].SetConstraint(_myVerlets[19]);
        _myVerlets[17].SetConstraint(_myVerlets[14]);
        _myVerlets[17].SetConstraint(_myVerlets[11]);
        _myVerlets[17].SetConstraint(_myVerlets[12]);
        _myVerlets[17].SetConstraint(_myVerlets[18]);
        _myVerlets[17].SetConstraint(_myVerlets[13]);

        _myVerlets[18].SetConstraint(_myVerlets[8]);
        _myVerlets[18].SetConstraint(_myVerlets[7]);
        _myVerlets[18].SetConstraint(_myVerlets[9]);
        _myVerlets[18].SetConstraint(_myVerlets[19]);

        _myVerlets[19].SetConstraint(_myVerlets[7]);
        _myVerlets[19].SetConstraint(_myVerlets[6]);
        _myVerlets[19].SetConstraint(_myVerlets[18]);
        _myVerlets[19].SetConstraint(_myVerlets[8]);
    }

    private void DrawLines()
    {
        _lines[0].SetPosition(0, transform.position + _myVerlets[0].CurrentPosition);
        _lines[0].SetPosition(1, transform.position + _myVerlets[1].CurrentPosition);
        _lines[1].SetPosition(0, transform.position + _myVerlets[1].CurrentPosition);
        _lines[1].SetPosition(1, transform.position + _myVerlets[2].CurrentPosition);
        _lines[2].SetPosition(0, transform.position + _myVerlets[2].CurrentPosition);
        _lines[2].SetPosition(1, transform.position + _myVerlets[3].CurrentPosition);
        _lines[3].SetPosition(0, transform.position + _myVerlets[3].CurrentPosition);
        _lines[3].SetPosition(1, transform.position + _myVerlets[4].CurrentPosition);
        _lines[4].SetPosition(0, transform.position + _myVerlets[4].CurrentPosition);
        _lines[4].SetPosition(1, transform.position + _myVerlets[5].CurrentPosition);
        _lines[5].SetPosition(0, transform.position + _myVerlets[5].CurrentPosition);
        _lines[5].SetPosition(1, transform.position + _myVerlets[6].CurrentPosition);
        _lines[6].SetPosition(0, transform.position + _myVerlets[6].CurrentPosition);
        _lines[6].SetPosition(1, transform.position + _myVerlets[7].CurrentPosition);
        _lines[7].SetPosition(0, transform.position + _myVerlets[7].CurrentPosition);
        _lines[7].SetPosition(1, transform.position + _myVerlets[19].CurrentPosition);
        _lines[8].SetPosition(0, transform.position + _myVerlets[7].CurrentPosition);
        _lines[8].SetPosition(1, transform.position + _myVerlets[8].CurrentPosition);
        _lines[9].SetPosition(0, transform.position + _myVerlets[8].CurrentPosition);
        _lines[9].SetPosition(1, transform.position + _myVerlets[18].CurrentPosition);
        _lines[10].SetPosition(0, transform.position + _myVerlets[8].CurrentPosition);
        _lines[10].SetPosition(1, transform.position + _myVerlets[9].CurrentPosition);
        _lines[11].SetPosition(0, transform.position + _myVerlets[9].CurrentPosition);
        _lines[11].SetPosition(1, transform.position + _myVerlets[10].CurrentPosition);
        _lines[12].SetPosition(0, transform.position + _myVerlets[10].CurrentPosition);
        _lines[12].SetPosition(1, transform.position + _myVerlets[11].CurrentPosition);
        _lines[13].SetPosition(0, transform.position + _myVerlets[11].CurrentPosition);
        _lines[13].SetPosition(1, transform.position + _myVerlets[12].CurrentPosition);
        _lines[14].SetPosition(0, transform.position + _myVerlets[12].CurrentPosition);
        _lines[14].SetPosition(1, transform.position + _myVerlets[13].CurrentPosition);
        _lines[15].SetPosition(0, transform.position + _myVerlets[13].CurrentPosition);
        _lines[15].SetPosition(1, transform.position + _myVerlets[14].CurrentPosition);
        _lines[16].SetPosition(0, transform.position + _myVerlets[13].CurrentPosition);
        _lines[16].SetPosition(1, transform.position + _myVerlets[15].CurrentPosition);
        _lines[17].SetPosition(0, transform.position + _myVerlets[14].CurrentPosition);
        _lines[17].SetPosition(1, transform.position + _myVerlets[15].CurrentPosition);
        _lines[18].SetPosition(0, transform.position + _myVerlets[15].CurrentPosition);
        _lines[18].SetPosition(1, transform.position + _myVerlets[0].CurrentPosition);
        _lines[19].SetPosition(0, transform.position + _myVerlets[0].CurrentPosition);
        _lines[19].SetPosition(1, transform.position + _myVerlets[17].CurrentPosition);
        _lines[20].SetPosition(0, transform.position + _myVerlets[17].CurrentPosition);
        _lines[20].SetPosition(1, transform.position + _myVerlets[2].CurrentPosition);
    }

    private void UpdateConstraints()
    {
        // Method name explains it all.
        for (int i = 0; i < 1; i++)
            ConstraintSolver.Update(_myVerlets, _myVerlets[0].TimeStep);
    }

    private void FixedUpdate()
    {
        // Get wind from Environment Forces every frame
        _wind = _forces.Wind;
        // Initialize the verlet constraints.
        // This happens only once, hacked it into the function to work that way.
        // Just couldn't do it before, nothing else worked!
        SetConsraints();
        // Applying forces to all verlets.
        // Forces: gravity, wind, air resistance, and applying its velocity too.
        foreach (var v in _myVerlets)
        {
            // Only apply forces if not collided.
            // Tried to do this to fix the issue where it manages to slip through the cracks of the mountain
            // However the bug still persists.
            if (v._hasCollided) continue;
            v.CurrentForce.y -= v.Mass * _gravity * v.TimeStep;
            v.CurrentForce -= v.GetVelocity() * -_wind * 20f * -_airResistance * v.TimeStep;
        }
        // Apply inertia to all verlets!
        foreach (var v in _myVerlets)
        {
            if (v._hasCollided) continue;
            var pPos = v.CurrentPosition;
            v.CurrentPosition = v.CurrentPosition + v.CurrentPosition - v.PreviousPosition +
                                v.CurrentForce * v.TimeStep * v.TimeStep;
            v.PreviousPosition = pPos;
        }

        // Update the constraints and then check for collision.
        // Made sure to update then check collision to avoid reforming of the verlets to
        // Make it fall into the mesh, however ...
        UpdateConstraints();
        DetectCollision();
        // Draw all lines between the points
        DrawLines();
        // Destroy the game object if beyond screen point
        foreach (var v in _myVerlets)
            if (v.transform.position.x > 18f || v.transform.position.x < -18f ||
                v.transform.position.y > 20f || v.transform.position.y < -5f)
                Destroy(gameObject);
    }

    private void DetectCollision()
    {
        foreach (var t in _myVerlets)
        {
            // Check against collision with mountain
            for (int j = 0; j < _mountainVertices.Count - 1; j++)
            {
                if (t._hasCollided) continue;
                var a = _mountainVertices[j];
                var b = _mountainVertices[j + 1];
                if (MyMath.PointLineDistance(a, b, t.transform.position) > 0.10f) continue;
                t.CurrentPosition = t.PreviousPosition;
                t.CurrentForce = Vector3.zero;
                t._hasCollided = true;
            }
            for (int j = 0; j < _groundVertices.Count - 1; j++)
            {

                if (t._hasCollided) continue;
                var a = _groundVertices[j];
                var b = _groundVertices[j + 1];
                if (MyMath.PointLineDistance(a, b, t.transform.position) > 0.30f) continue;
                t.CurrentPosition = t.PreviousPosition;
                t.CurrentForce = Vector3.zero;
                t._hasCollided = true;
                StartCoroutine(DestroyGoat());
            }

            foreach (var physicsEngine_2D in ProjectileManager.Projectiles)
            {
                if (Vector3.Magnitude(t.transform.position - physicsEngine_2D.transform.position) > 0.3f) continue;
                t.CurrentPosition += physicsEngine_2D.GetVelocity() * t.TimeStep;
                t.CurrentForce = Vector3.zero;
                StartCoroutine(physicsEngine_2D.DestroyProjectile(0.5f));
            }
            
        }
    }

    private IEnumerator DestroyGoat()
    {
        // For destroying game object upon contact with ground mesh
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }

    private void ExtractMeshVertices()
    {
        var mountLeftVertices =
            _mountain.GetComponentInChildren<MountainLeft>().gameObject.GetComponent<MeshFilter>().mesh.vertices;
        var mountRightVertices =
            _mountain.GetComponentInChildren<MountainRight>().gameObject.GetComponent<MeshFilter>().mesh.vertices;
        var mountMidVertices =
            _mountain.GetComponentInChildren<MountainMiddle>().gameObject.GetComponent<MeshFilter>().mesh.vertices;
        foreach (var t in mountLeftVertices)
            _mountainVertices.Add(t);
        foreach (var t in mountRightVertices)
            _mountainVertices.Add(t);
        foreach (var t in mountMidVertices)
            _mountainVertices.Add(t);
    }

    private void ExtractGroundVertices()
    {
        var groundVertices = _ground.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        foreach (var vertex in groundVertices)
            _groundVertices.Add(vertex);
    }
}