using UnityEngine;
using System.Collections.Generic;

public class Rules : MonoBehaviour {

    public List<BB> boids;

    public GameObject prefab;
    public int numBoid;
    public float maxD;
    public float boundaries;
    public Transform target;

    public float mMax;
    public float mMin;

    

    [Range(0.0f, 1.0f)]
    public float cohesion;

    [Range(0.0f, 1.0f)]
    public float dispersion;

    [Range(0.0f, 1.0f)]
    public float alignment;

    public void CV(Vector3 vec)
    {
        vec.x = Mathf.Clamp(vec.x, 0, 1);
        vec.y = Mathf.Clamp(vec.y, 0, 1);
        vec.z = Mathf.Clamp(vec.z, 0, 1);
    }

    public void Start()
    {
        boids = new List<BB>();
        Vector3 pos = Vector3.zero;
        for(int i = 0; i < numBoid; i++)
        {
            pos.x = Random.Range(-maxD, maxD);
            pos.y = Random.Range(-maxD, maxD);
            pos.z = Random.Range(-maxD, maxD);

            GameObject temp = Instantiate(prefab, pos, new Quaternion()) as GameObject;

            BB bb = temp.GetComponent<BB>();
            bb.velocity = bb.transform.position.normalized;
            bb.mass = Random.Range(mMin, mMax);

            bb.transform.parent = transform;

            boids.Add(bb);
        }

        //foreach (BB bb in FindObjectsOfType<BB>())
        //{
        //    if (boids.Contains(bb) == false)
        //        boids.Add(bb);
        //}
    }

    private void FixedUpdate()
    {
        foreach (BB bb in boids)
        {
            Vector3 r1 = COM(bb) * cohesion;
            Vector3 r2 = Dispersion(bb) * dispersion;
            Vector3 r3 = Alignment(bb) * alignment;
            Vector3 bound = BoundPosition(bb);
            bb.velocity += (r1 + r2 + r3 + bound) / bb.mass;

        }
    }

    //Rule 1 Center of Mass
    private Vector3 COM(BB b)
    {
        Vector3 pc = Vector3.zero;
        float totalMass = 0;
        foreach (BB bj in boids)
        {
            if(bj != b)
            {
                pc += bj.transform.position * bj.mass;
                totalMass += bj.mass;
            }
        }

        pc = pc / totalMass;

       return (pc - b.transform.position).normalized;
    }
    //Rule 2 Dispersion
    private Vector3 Dispersion(BB b)
    {
        Vector3 avoid = Vector3.zero;
        foreach(BB bj in boids)
        {
            if((bj.transform.position - b.transform.position).magnitude <= 50 && bj != b)
            {
                avoid -= bj.transform.position - b.transform.position;
            }
        }
        return avoid.normalized;
    }

    private Vector3 Alignment(BB b)
    {
        Vector3 pv = Vector3.zero;
        foreach(BB bj in boids)
        {
            if (bj != b)
                pv += bj.velocity;
        }
        pv = pv / (boids.Count - 1);
        Vector3 rule3 = (pv - b.velocity).normalized;

        CV(rule3);

        return rule3;

        
    }

    private Vector3 BoundPosition(BB b)
    {
        Vector3 BPos = new Vector3();

        if (b.transform.position.x > boundaries)
            BPos += new Vector3(-5, 0, 0);
        else if (b.transform.position.x < -boundaries)
            BPos += new Vector3(5, 0, 0);

        if (b.transform.position.y > boundaries)
            BPos += new Vector3(0, -5, 0);
        else if (b.transform.position.y < -boundaries)
            BPos += new Vector3(0, 5, 0);

        if (b.transform.position.z > boundaries)
            BPos += new Vector3(0, 0, -5);
        else if (b.transform.position.z < -boundaries)
            BPos += new Vector3(0, 0, 5);
        


        return BPos;
    }

}
