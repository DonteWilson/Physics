using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Particle {
    Particle()
    {

    }


    
    public Particle(Vector3 p, Vector3 v, float mass)
    {
        _position = p;
        _velocity = v;
        m_mass = mass;
        _force = Vector3.zero;

    }

    public void AddForce(Vector3 force)
    {
        Force += force;
    }

    public bool Kinematic;
    Vector3 _position;
    Vector3 _acceleration;
    Vector3 _force;
    Vector3 _velocity;

    float m_mass;

    public float Mass
    {
        get { return m_mass; }
        set { m_mass = value; }
    }
    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public Vector3 Force
    {
        get { return _force; }
        set { _force = value; }
    }

    public Vector3 Velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    public void Update()
    {
        if (Kinematic)
            return;
        _acceleration = (1f / m_mass) * Force;
        _velocity +=  _acceleration * Time.fixedDeltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, 3.0f);
        _position += _velocity * Time.fixedDeltaTime;
    }
}
public class SpringDamper
{
    public Particle p1, p2, p3;
    public float Ks;
    public float Kd;
    public float Lo;
    
  

    public SpringDamper(Particle P1, Particle P2, float SpringK, float SpringD, float SpringR)
    {
        Ks = SpringK;
        Kd = SpringD;
        Lo = SpringR;
        p1 = P1;
        p2 = P2;
    }
    public void ComputeForce()
    {
        Vector3 dist = (p2.Position - p1.Position);
        Vector3 distD = dist.normalized;

        float p11D = Vector3.Dot(distD, p1.Velocity);
        float p21D = Vector3.Dot(distD, p2.Velocity);


        float Fs = -Ks * (Lo - dist.magnitude);
        float Fd = -Kd * (p11D - p21D);

        Vector3 SpringForce = (Fs + Fd) * distD;

        //adds force to the spring
        p1.AddForce(SpringForce);
        p2.AddForce(-SpringForce);

    }

}

[Serializable]
public class Triangle
{
    public Vector3 Surfnorm;
    public Vector3 AverageV;
    public float AreaTri;
    public float WindCoeff = 1f;
    public Particle P1, P2, P3;
    public SpringDamper D1, D2, D3;

    public Triangle() { }

    public Triangle(Particle pOne, Particle pTwo, Particle pThree)
    {
        P1 = pOne;
        P2 = pTwo;
        P3 = pThree;
    }

    public void ComputeAD(Vector3 air)
    {
        Vector3 surface = ((P1.Velocity + P2.Velocity + P3.Velocity) / 3);
        AverageV = surface - air;
        Surfnorm = Vector3.Cross((P2.Position - P1.Position), (P3.Position - P1.Position)) /
        Vector3.Cross((P2.Position - P1.Position), (P3.Position - P1.Position)).magnitude;
        float ao = (1f / 2f) * Vector3.Cross((P2.Position - P1.Position), (P3.Position - P1.Position)).magnitude;
        AreaTri = ao * (Vector3.Dot(AverageV, Surfnorm) / AverageV.magnitude);
        Vector3 aeroForce = -(1f / 2f) * 1f * Mathf.Pow(AverageV.magnitude, 2) * 1f * AreaTri * Surfnorm;
        P1.AddForce(aeroForce / 3);
        P2.AddForce(aeroForce / 3);
        P3.AddForce(aeroForce / 3);

        
    }

    //public void Draw()
    //{
     
    //    Debug.DrawLine(P3.Position, P1.Position, Color.cyan);
    //}
}
   
  


   