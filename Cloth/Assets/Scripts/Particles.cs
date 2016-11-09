using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Particles {
    Particles()
    {

    }


    
    public Particles(Vector3 p, Vector3 v, float mass)
    {
        m_Position = p;
        m_Velocity = v;
        m_mass = mass;
        m_Force = Vector3.zero;
        m_Momentum = Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        Force += force;
    }

    public bool Kinematic;
    Vector3 m_Position;
    Vector3 m_Acceleration;
    Vector3 m_Momentum;
    Vector3 m_Force;
    Vector3 m_Velocity;

    float m_mass;

    public float Mass
    {
        get { return m_mass; }
        set { m_mass = value; }
    }
    public Vector3 Position
    {
        get { return m_Position; }
        set { m_Position = value; }
    }

    public Vector3 Force
    {
        get { return m_Force; }
        set { m_Force = value; }
    }

    public Vector3 Velocity
    {
        get { return m_Velocity; }
        set { m_Velocity = value; }
    }

    public void Update()
    {
        if (Kinematic)
            return;
        m_Acceleration = (1f / m_mass) * Force;
        m_Velocity += m_Acceleration * Time.fixedDeltaTime;
        m_Position += Vector3.ClampMagnitude(m_Velocity, 5) + m_Velocity * Time.fixedDeltaTime;
    }
}
public class SpringDamper
{
    public Particles p1, p2, p3;
    public float Ks;
    public float Kd;
    public float Lo;
    public SpringDamper(Particles P1, Particles P2, float SpringK, float SpringD, float SpringR)
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

    public void Draw()
    {
        //spring.SetPosition(0, p1.Position);
        //spring.SetPosition(1, p2.Position);
        Debug.DrawLine(p1.Position, p2.Position, Color.cyan);
       

    }
}

public class Triangle
{
    public Vector3 surfnorm;
    public Vector3 averageV;
    public float areaTri;
    public float windCoeff = 1f;
    public Particles P1, P2, P3;
    public SpringDamper D1, D2, D3;

    public Triangle() { }

    public Triangle(Particles pOne, Particles pTwo, Particles pThree)
    {
        P1 = pOne;
        P2 = pTwo;
        P3 = pThree;
    }

    public void ComputeAD(Vector3 air)
    {
        Vector3 surface = ((P1.Velocity + P2.Velocity + P3.Velocity) / 3);
        averageV = surface - air;
        surfnorm = Vector3.Cross((P2.Position - P1.Position), (P3.Position - P1.Position)) /
        Vector3.Cross((P2.Position - P1.Position), (P3.Position - P1.Position)).magnitude;
        float ao = (1f / 2f) * Vector3.Cross((P2.Position - P1.Position), (P3.Position - P1.Position)).magnitude;
        areaTri = ao * (Vector3.Dot(averageV, surfnorm) / averageV.magnitude);
        Vector3 aeroForce = -(1f / 2f) * 1f * Mathf.Pow(averageV.magnitude, 2) * 1f * areaTri * surfnorm;
        P1.AddForce(aeroForce / 3);
        P2.AddForce(aeroForce / 3);
        P3.AddForce(aeroForce / 3);

        
    }

    public void Draw()
    {
        Debug.DrawLine(P3.Position, P1.Position, Color.cyan);
    }
}
   
  


   