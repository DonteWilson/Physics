﻿using UnityEngine;
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
        m_Position += m_Velocity * Time.fixedDeltaTime;
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
    
    public LineRenderer spring;
    public void Draw()
    {
        //spring.SetPosition(0, p1.Position);
        //spring.SetPosition(1, p2.Position);
        Debug.DrawLine(p1.Position, p2.Position, Color.cyan);
       
    }
}

   