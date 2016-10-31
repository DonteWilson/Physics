using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Particle
{
    public Vector3 pos;
    public Vector3 velocity;
    public float mass;

    public void Update()
    {

    }

}
class SpringDampler
{
    float SpringConstant;//Ks
    float DampingFactor; //Kd
    float RestLength; //lo
    ParticleSystem P1, P2;
}




