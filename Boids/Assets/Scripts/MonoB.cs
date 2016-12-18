using UnityEngine;
using System.Collections;
using Inter;

public class MonoB : MonoBehaviour {

    public Boids boid;
    public float mass;

    public void Awake()
    {
        boid = new Boids(mass);

    }

    public void LateUpdate()
    {
        boid.UpdateVelocity();
        transform.position = boid.position;
    }
}
