using System;
using UnityEngine;


/// <summary>
/// Public class Particle
/// </summary>
[Serializable]
public class Particle
{

    public bool Kinematic;

    public bool Click;
    public float mMass;
    /// <summary>
    /// Vector variables
    /// </summary>
    public Vector3 _position;
    public Vector3 _acceleration;
    public Vector3 _force;
    public Vector3 _velocity;

    /// <summary>
    /// Initializes a new instance of the Particle class
    /// </summary>
    /// <param name="p">The Position</param>
    /// <param name="v">The Velocity</param>
    /// <param name="mass">The Mass</param>
    public Particle(Vector3 p, Vector3 v, float mass)
    {
        _position = p;
        _velocity = v;
        mMass = mass;
        _force = Vector3.zero;

    }



    /// <summary>
    /// Prevents a default instance of the Particle class from being created.
    /// </summary>
    private Particle()
    {
    }

    /// <summary>
    /// Adds Force
    /// </summary>
    /// <param name="force">Applies Force</param>
    public void AddForce(Vector3 force)
    {
        Force += force;
    }


    /// <summary>
    /// Gets or sets the mass
    /// </summary>
    public float Mass
    {
        get { return mMass; }
        set { mMass = value; }
    }

    /// <summary>
    /// Gets or sets the position
    /// </summary>
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    /// <summary>
    /// Gets or sets the force
    /// </summary>
    public Vector3 Force
    {
        get { return _force; }
        set { _force = value; }
    }

    /// <summary>
    /// Gets or sets the velocity
    /// </summary>
    public Vector3 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }


  
    /// <summary>
    /// Update functions
    /// </summary>
    public void Update()
    {
        if (Kinematic)
            return;

        _acceleration = (1f / mMass) * Force;
        _velocity += _acceleration * Time.fixedDeltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, 3.0f);
        _position += _velocity * Time.fixedDeltaTime;        
    }


}

/// <summary>
/// Spring Damper class
/// </summary>
[Serializable]
public class SpringDamper
{
    public Particle p1, p2;
    public float Ks;
    public float Kd;
    public float Lo;
    public float L;

    


    /// <summary>
    /// Initializes a new instance of the SpringDamper class.
    /// </summary>
    /// <param name="pOne">Particle One</param>
    /// <param name="pTwo">Particle Two</param>
    /// <param name="springK">Spring Ks</param>
    /// <param name="springD">Spring Damper</param>
    /// <param name="springR">Spring Rest</param>
    public SpringDamper(Particle pOne, Particle pTwo, float springK, float springD, float springR)
    {

        p1 = pOne;
        p2 = pTwo;
        Ks = springK;
        Kd = springD;
        Lo = springR;
    }


    /// <summary>
    /// Compute Force on springDampers
    /// </summary>
    public void ComputeForce()
    {
        Vector3 dist = p2.Position - p1.Position;
        Vector3 distD = dist.normalized;

        float p11D = Vector3.Dot(distD, p1.Velocity);
        float p21D = Vector3.Dot(distD, p2.Velocity);


        float fs = -Ks * (Lo - dist.magnitude);
        float fd = -Kd * (p11D - p21D);

        Vector3 SpringForce = (fs + fd) * distD;

        //adds force to the spring
        p1.AddForce(SpringForce);
        p2.AddForce(-SpringForce);

        L = dist.magnitude;

    }

}

[Serializable]

/// <summary>
/// Triangle Class
/// </summary>
public class Triangle
{
    public SpringDamper D1, D2, D3;
    public Vector3 surfnorm;
    public Vector3 averageV;
    public float areaTri;
    public float windCoeff = 1f;
    public Particle p1, p2, p3;

    /// <summary>
    /// Initializes a new instance of the Triangle class
    /// </summary>
    public Triangle()
    {        
    }

    /// <summary>
    /// Initializes a new instance of the Triangle class
    /// </summary>
    /// <param name="pOne">Particle one</param>
    /// <param name="pTwo">Particle two</param>
    /// <param name="pThree">Particle three</param>
    public Triangle(Particle pOne, Particle pTwo, Particle pThree)
    {
        p1 = pOne;
        p2 = pTwo;
        p3 = pThree;
    }

    /// <summary>
    /// Computes air forces
    /// </summary>
    /// <param name="air">Vector3 air</param>
    public void ComputeAd(Vector3 air)
    {
        Vector3 surface = (p1.Velocity + p2.Velocity + p3.Velocity) / 3;
        averageV = surface - air;
        surfnorm = Vector3.Cross(p2.Position - p1.Position, p3.Position - p1.Position) /
        Vector3.Cross(p2.Position - p1.Position, p3.Position - p1.Position).magnitude;
        float ao = (1f / 2f) * Vector3.Cross(p2.Position - p1.Position, p3.Position - p1.Position).magnitude;
        areaTri = ao * (Vector3.Dot(averageV, surfnorm) / averageV.magnitude);
        Vector3 aeroForce = -(1f / 2f) * 1f * Mathf.Pow(averageV.magnitude, 2) * 1f * areaTri * surfnorm;
        p1.AddForce(aeroForce / 3);
        p2.AddForce(aeroForce / 3);
        p3.AddForce(aeroForce / 3);      
    }
}