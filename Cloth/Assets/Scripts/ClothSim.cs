using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClothSim : MonoBehaviour
{
    GameObject Sphere1, Sphere2;
    List<Particles> particles;
    List<SpringDamper> springDampers;
    public float Ks, Kd, Lo;
    void Start()
    {
        particles = new List<Particles>();
        springDampers = new List<SpringDamper>();
        Sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Particles p1 = new Particles(Vector3.zero, Vector3.zero, 1f);
        p1.Kinematic = true;

        Particles p2 = new Particles(Vector3.down, Vector3.zero, 1f);

        particles.Add(p1);
        particles.Add(p2);

        SpringDamper sd = new SpringDamper(p1, p2, Ks, Kd, Lo);
        springDampers.Add(sd);   
    }
    public float slider = 0;
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
            p.Force = Vector3.down * p.Mass;
        }

        foreach (SpringDamper sd in springDampers)
        {
            sd.ComputeForce();
        }
    }

    void LateUpdate()
    {
        Sphere1.transform.position = particles[0].Position;
        Sphere2.transform.position = particles[1].Position;

        foreach (Particles p in particles)
        {
            p.Update();
        }
    }
}



