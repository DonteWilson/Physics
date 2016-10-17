using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Agent : MonoBehaviour
{
    public Vector3 force;
    public Vector3 desiredv;
    public Vector3 velocity;
    public int mass;
    public Vector3 steering;
    public Transform target;

    public float arrival;
    private float radius;

    void start()
    {
        arrival = 1;
    }


	// Use this for initialization
	void FixedUpdate () {
        steering = (desiredv - velocity).normalized;
        desiredv = (target.position - transform.position).normalized / mass;
        velocity += steering;
        if (velocity.magnitude > 5)
            velocity = velocity.normalized;

        float dist = Vector3.Distance(target.transform.position, transform.position);

        arrival = (dist <= radius) ? dist / radius : 1;

    }
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position += velocity;

        velocity *= arrival;
        
	
	}
}
