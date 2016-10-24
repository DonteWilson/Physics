using UnityEngine;
using System.Collections;

public class SA : MonoBehaviour {

    MonoB m;
    Vector3 desiredV;
    public Transform target;
    Vector3 steering;
    public float steeringF;
    public float radius;

    void Start()
    {
        m = gameObject.GetComponent<MonoB>();
    }

    void FixedUpdate()
    {
        float pushbackFactor = (target.position - transform.position).magnitude / radius;
        Vector3 pushBackForce = (target.position - transform.position).normalized * pushbackFactor;

        m.boid.velocity = pushBackForce / m.boid.mass;

        if (m.boid.velocity.magnitude > 3)
            m.boid.velocity = m.boid.velocity.normalized;
    }
	
	
}
