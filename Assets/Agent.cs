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


	// Use this for initialization
	void FixedUpdate () {
        steering = (desiredv - velocity).normalized;
        desiredv = (target.position - transform.position).normalized / mass;
        velocity += steering;
        if (velocity.magnitude > 5)
            velocity = velocity.normalized;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position += velocity;
        
        
	
	}
}
