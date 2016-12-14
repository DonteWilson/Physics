using System;
using UnityEngine;

/// <summary>
/// Public class Particle
/// </summary>
[Serializable]
public class Particle
{

    public bool Kinematic;
    public float mMass;
    /// <summary>
    /// Vector variables
    /// </summary>
    public Vector3 position;
    public Vector3 acceleration;
    public Vector3 force;
    public Vector3 velocity;

    /// <summary>
    /// Initializes a new instance of the Particle class
    /// </summary>
    /// <param name="p">The Position</param>
    /// <param name="v">The Velocity</param>
    /// <param name="mass">The Mass</param>
    public Particle(Vector3 p, Vector3 v, float mass)
    {
        this.position = p;
        this.velocity = v;
        this.mMass = mass;
        this.force = Vector3.zero;

    }

    /// <summary>
    /// Prevents a default instance of the Particle class from being created.
    /// </summary>
    private Particle()
    {
    }

    /// <summary>
    /// Gets or sets the mass
    /// </summary>
    public float Mass
    {
        get { return this.mMass; }
        set { this.mMass = value; }
    }

    /// <summary>
    /// Gets or sets the position
    /// </summary>
    public Vector3 Position
    {
        get { return this.position; }
        set { this.position = value; }
    }

    /// <summary>
    /// Gets or sets the force
    /// </summary>
    public Vector3 Force
    {
        get { return this.force; }
        set { this.force = value; }
    }

    /// <summary>
    /// Gets or sets the velocity
    /// </summary>
    public Vector3 Velocity
    {
        get { return this.velocity; }
        set { this.velocity = value; }
    }


    /// <summary>
    /// Update functions
    /// </summary>
    public void Update()
    {
        if (this.Kinematic)
            return;

        this.acceleration = (1f / this.mMass) * this.Force;
        this.velocity += this.acceleration * Time.fixedDeltaTime;
        this.velocity = Vector3.ClampMagnitude(this.velocity, 3.0f);
        this.position += this.velocity * Time.fixedDeltaTime;        
    }


    /// <summary>
    /// Adds Force
    /// </summary>
    /// <param name="force">Applies Force</param>
    public void AddForce(Vector3 Force)
    {
        this.Force += this.force;
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

        this.p1 = pOne;
        this.p2 = pTwo;
        this.Ks = springK;
        this.Kd = springD;
        this.Lo = springR;
    }


    /// <summary>
    /// Compute Force on springDampers
    /// </summary>
    public void ComputeForce()
    {
        Vector3 dist = this.p2.Position - this.p1.Position;
        Vector3 distD = dist.normalized;

        float p11D = Vector3.Dot(distD, this.p1.Velocity);
        float p21D = Vector3.Dot(distD, this.p2.Velocity);


        float fs = -this.Ks * (this.Lo - dist.magnitude);
        float fd = -this.Kd * (p11D - p21D);

        Vector3 SpringForce = (fs + fd) * distD;

        //adds force to the spring
        this.p1.AddForce(SpringForce);
        this.p2.AddForce(-SpringForce);

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
        this.p1 = pOne;
        this.p2 = pTwo;
        this.p3 = pThree;
    }

    /// <summary>
    /// Computes air forces
    /// </summary>
    /// <param name="air">Vector3 air</param>
    public void ComputeAd(Vector3 air)
    {
        Vector3 surface = (this.p1.Velocity + this.p2.Velocity + this.p3.Velocity) / 3;
        this.averageV = surface - air;
        this.surfnorm = Vector3.Cross(this.p2.Position - this.p1.Position, this.p3.Position - this.p1.Position) /
        Vector3.Cross(this.p2.Position - this.p1.Position, this.p3.Position - this.p1.Position).magnitude;
        float ao = (1f / 2f) * Vector3.Cross(this.p2.Position - this.p1.Position, this.p3.Position - this.p1.Position).magnitude;
        this.areaTri = ao * (Vector3.Dot(this.averageV, this.surfnorm) / this.averageV.magnitude);
        Vector3 aeroForce = -(1f / 2f) * 1f * Mathf.Pow(this.averageV.magnitude, 2) * 1f * this.areaTri * this.surfnorm;
        this.p1.AddForce(aeroForce / 3);
        this.p2.AddForce(aeroForce / 3);
        this.p3.AddForce(aeroForce / 3);      
    }
}