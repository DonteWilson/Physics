using UnityEngine;


public class Agent : MonoBehaviour
{
    /// <summary>
    /// Contains Vector3 force
    /// </summary>
    private Vector3 force;

    /// <summary>
    /// Contains Vector3 desired force 
    /// </summary>
    private Vector3 desiredv;

    /// <summary>
    /// Contains Vector3 velocity
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// Contains integer for mass
    /// </summary>
    public int mass;

    private Vector3 steering;

    /// <summary>
    /// Creates a transform for target
    /// </summary>
    public Transform target;

    /// <summary>
    /// private float arrival
    /// </summary>
    private float arrival;

    /// <summary>
    /// float radius
    /// </summary>
    private float radius;

    /// <summary>
    /// Start update
    /// </summary>
    private void Start()
    {
        this.arrival = 1;
    }

    /// <summary>
    /// Fixed Update
    /// </summary>
    private void FixedUpdate()
    {
        this.steering = (this.desiredv - this.velocity).normalized;
        this.desiredv = (this.target.position - this.transform.position).normalized / this.mass;
        this.velocity += this.steering;
        if (this.velocity.magnitude > 5)
        {
            this.velocity = this.velocity.normalized;
        }

        float dist = Vector3.Distance(this.target.transform.position, this.transform.position);

        this.arrival = (dist <= this.radius) ? dist / this.radius : 1;
    } 

    /// <summary>
    /// Late Update
    /// </summary>
    private void LateUpdate()
    {
        this.transform.position += this.velocity;
        this.velocity *= this.arrival;
    }
}
