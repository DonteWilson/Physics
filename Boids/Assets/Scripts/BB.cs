using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BB : MonoBehaviour {

    Vector3 velocity;
    public float mass;

    Vector3 rule1;
    Vector3 rule2;
    Vector3 rule3;

    [Range(0.0f, 1.0f)]
    public float cohesion;

    [Range(0.0f, 1.0f)]
    public float dispersion;

    [Range(0.0f, 1.0f)]
    public float allignment;

    List<BB> Boids;

    void Start()
    {
        Boids = new List<BB>();
            foreach(BB bb in FindObjectsOfType<BB>())
        {
            if(this != bb)
            {
                Boids.Add(bb);
            }
        }

        rule1 = Vector3.zero;
        rule2 = Vector3.zero;
        rule3 = Vector3.zero;

    }

    void FixedUpdate()
    {

    }

    void LateUpdate()
    {

    }

}
