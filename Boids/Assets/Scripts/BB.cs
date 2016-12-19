using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BB : MonoBehaviour {

    [HideInInspector]public Vector3 velocity;

    public float mass;

    public void LateUpdate()
    {
        transform.position += velocity;
        transform.forward = velocity.normalized;
    }
}
