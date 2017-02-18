using UnityEngine;

public class Verlet
{
//private:
    private float _angle;
    private float _wind;
    private float _initialVelocity;
    private float _gravity;
    private float _airResistance;
    private Vector3 _velocity;
    private Vector3 _acceleration;
    private Vector3 _lastPosition;
    private Vector3 _currentPosition;
    private Vector3 _forces;
//public:
    public GameObject Point;

    public Verlet(float angle, float wind, float airResistance, Vector3 position)
    {
        _angle = angle;
        _wind = wind;
        _gravity = 9.81f;
        _airResistance = airResistance;
        _velocity = Vector3.zero;
        _acceleration = Vector3.zero;
        _velocity = new Vector3(_initialVelocity * Mathf.Cos(Mathf.Deg2Rad * _angle),
            _initialVelocity * Mathf.Sin(Mathf.Deg2Rad * _angle));
        _currentPosition = position;
        _lastPosition = _currentPosition - _velocity;
        if (_wind > 0 || _wind < 0)
        {
            _forces = new Vector3(-_wind + _airResistance, _airResistance + _gravity);
        }
        Point.transform.position = _currentPosition;
    }

    public void Update(float deltaTime)
    {
        _lastPosition = _currentPosition;
        _currentPosition = _currentPosition + (_currentPosition - _lastPosition) + _forces * Mathf.Pow(deltaTime, 2);
    }

    public float GetAngle()
    {
        return _angle;
    }

    public float GetWind()
    {
        return _wind;
    }

    public float GetInitialVelocity()
    {
        return _initialVelocity;
    }

    public float GetGravity()
    {
        return _gravity;
    }

    public float GetAirResistance()
    {
        return _airResistance;
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    public Vector3 GetLastPosition()
    {
        return _lastPosition;
    }

    public Vector3 GetCurrentPosition()
    {
        return _currentPosition;
    }

    public Vector3 GetForces()
    {
        return _forces;
    }

    public void SetAngle(float angle)
    {
        _angle = angle;
    }

    public void SetWind(float wind)
    {
        _wind = wind;
    }

    public void SetInitialVelocity(float initialVelocity)
    {
        _initialVelocity = initialVelocity;
    }

    public void SetGravity(float gravity)
    {
        _gravity = gravity;
    }

    public void SetAirResistance(float airResistance)
    {
        _airResistance = airResistance;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void SetLastPosition(Vector3 lastPosition)
    {
        _lastPosition = lastPosition;
    }

    public void SetCurrentPosition(Vector3 currentPosition)
    {
        _currentPosition = currentPosition;
    }

    public void SetForces(Vector3 forces)
    {
        _forces = forces;
    }
}
