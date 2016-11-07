﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClothSim : MonoBehaviour
{
    GameObject Sphere1, Sphere2;
    List<Particles> particles;
    List<SpringDamper> springDampers;
    List<GameObject> gameObjects;
    List<Triangle> aeroDynamics;
    public bool wind;
  
    public LineRenderer spring;
    [Range(0.0f, 5)]
    public float Ks, Kd, Lo;
    public float Gravity = 5f;
    [Range(0.0f, 5f)]
    public int height;
    [Range(0.0f, 5)]
    public int width;
    [Range(0.0f, 5)]
    public float slider = 0;
    [Range(0.01f, 10f)]
    public float windz;
    void Start()
    {
        particles = new List<Particles>();
        springDampers = new List<SpringDamper>();
        gameObjects = new List<GameObject>();
        aeroDynamics = new List<Triangle>();

        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                Particles p = new Particles(new Vector3(x * 5, -y, 0), Vector3.zero, 1f);
                go.transform.position = p.Position;
                gameObjects.Add(go);
                go.name = "Particle::" + (gameObjects.Count - 1).ToString();
                particles.Add(p);
            }
        }

        for (int i = 0; i < particles.Count; ++i)
        {
            if(i % (width) != width -1)
            {
                SpringDamper sdRight = new SpringDamper(particles[i], particles[i + 1], Ks, Kd, Lo);
                springDampers.Add(sdRight);
            }
            if(i < (particles.Count - height))
            {
                SpringDamper sdDown = new SpringDamper(particles[i], particles[i + height], Ks, Kd, Lo);
                springDampers.Add(sdDown);
            }
        }

        particles[0].Kinematic = true;
        particles[width - 1].Kinematic = true;
        //Sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Particles p1 = new Particles(Vector3.zero, Vector3.zero, 1f);
        //p1.Kinematic = true;

        //Particles p2 = new Particles(Vector3.down, Vector3.zero, 1f);

        //particles.Add(p1);
        //particles.Add(p2);

        //SpringDamper sd = new SpringDamper(p1, p2, Ks, Kd, Lo);
        //springDampers.Add(sd);   
    }
  
    void Update()
    {
        particles[0].Position = new Vector3(slider, 0, 0);
        foreach(SpringDamper sd in springDampers)
        {
            sd.Lo = Lo;
            sd.Ks = Ks;
            sd.Kd = Kd;
        }
    }

    void FixedUpdate()
    {
        foreach (Particles p in particles)
        {
            p.Force = Vector3.down * Gravity * p.Mass;
           
        }
        foreach (Triangle t in aeroDynamics)
        {
            aeroDynamics.Add(t);
        }

        foreach (SpringDamper sd in springDampers)
        {
            sd.ComputeForce();
            //spring.SetPosition(0,  )
            sd.Draw();
        }

        foreach (Triangle t in aeroDynamics)
        {
            if(wind)
            {
                if(!springDampers.Contains(t.D1) || !springDampers.Contains(t.D2) || !springDampers.Contains(t.D3))
                {
                    aeroDynamics.Remove(t);
                }
                else
                {
                    t.ComputeAD(Vector3.forward * windz);
                }
            }
        }
    }

    void LateUpdate()
    {
       for(int i = 0; i < particles.Count; ++i)
        {
            gameObjects[i].transform.position = particles[i].Position;
        }

        foreach (Particles p in particles)
        {
            p.Update();
        }
    }
}


