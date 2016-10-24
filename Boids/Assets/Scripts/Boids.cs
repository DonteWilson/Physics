using UnityEngine;
using System.Collections;
using Inter;
using System.Collections.Generic;

public class Boids : iMove  {

    private float m_mass;
    private Vector3 m_velocity;
    private Vector3 m_position;
    public float mass
    {
        get { return m_mass; }
        set { m_mass = value; }
    }

    public Vector3 position
    {
        get { return m_position; }
        set { m_position = value; }
    }

    public Vector3 velocity
    {
        get { return m_velocity; }
        set { m_velocity = value; }
    }

    public Boids(float m)
    {
        velocity = Vector3.zero;
        position = Vector3.zero;
        mass = (mass == 0) ? 1 : mass;
    }

    public void UpdateVelocity()
    {
        position += velocity;
    }
}
