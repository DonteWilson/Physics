using UnityEngine;
using System.Collections;

public class MonoParticle : MonoBehaviour {
    public Particle particle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = particle.Position;	
	}
}
