using UnityEngine;
using System.Collections;
using Inter;

public class MonoB : MonoBehaviour {

    public Boids boid;
    public float mass;

    void Awake()
    {
        boid = new Boids(mass);

    }

    void LateUpdate()
    {
        boid.UpdateVelocity();
        transform.position = boid.position;
    }
}
