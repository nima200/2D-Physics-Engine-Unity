using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verlet : MonoBehaviour
{
    public Vector3 InitialPosition;
    public Vector3 CurrentPosition;
    public Vector3 PreviousPosition;
    public Vector3 CurrentForce;
    public float Mass;
    public float TimeStep;
    public int Index;

    public Vector3 _velocity;
    public bool _hasCollided;

    private void Awake()
    {
        _velocity = Vector3.zero;
        gameObject.name = "Verlet";
        Mass = 1.0f;
        _hasCollided = false;
    }

    private void Start()
    {
        transform.SetParent(transform, false);
        transform.position = transform.parent.position + InitialPosition;
        CurrentPosition = InitialPosition;
    }

    private void Update()
    {
        transform.position = transform.parent.position + CurrentPosition;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void SetConstraint(Verlet v)
    {
        ConstraintSolver.Set(Index, v.Index, CurrentPosition, v.CurrentPosition);
    }

    public void SetPreviousPosition()
    {
        PreviousPosition = InitialPosition - _velocity * TimeStep;
    }
}
