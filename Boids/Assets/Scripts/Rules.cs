using UnityEngine;
using System.Collections.Generic;

public class Rules : MonoBehaviour {

    public List<BB> boids;

    public GameObject prefab;
    public int numBoid;
    public float maxD;
    public float boundaries;
    public Transform target;
    public Transform center;
    public float tRange;

    public float mMax;
    public float mMin;

    

    [Range(0.0f, 1.0f)]
    public float cohesion;

    [Range(0.0f, 1.0f)]
    public float dispersion;

    [Range(0.0f, 1.0f)]
    public float alignment;

    [Range(-1.0f,1.0f)]
    public float tendency;

    [Range(0.0f, 2.0f)]
    public float lim;

    public void CV(Vector3 vec)
    {
        vec.x = Mathf.Clamp(vec.x, 0, 1);
        vec.y = Mathf.Clamp(vec.y, 0, 1);
        vec.z = Mathf.Clamp(vec.z, 0, 1);
    }

    public void Awake()
    {
 
        boids = new List<BB>();
        Vector3 pos = Vector3.zero;
        for(int i = 0; i < numBoid; i++)
        {
            pos.x = Random.Range(-maxD, maxD);
            pos.y = Random.Range(-maxD, maxD);
            pos.z = Random.Range(-maxD, maxD);

            GameObject temp = Instantiate(prefab, transform.position + pos, new Quaternion()) as GameObject;

            BB bb = temp.GetComponent<BB>();
            bb.velocity = bb.transform.position.normalized;
            bb.mass = Random.Range(mMin, mMax);

            bb.transform.parent = transform;

            boids.Add(bb);
        }
    }

    public void FixedUpdate()
    {
        foreach (BB bb in boids)
        {
            Vector3 r1 = Cohesion(bb) * cohesion;
            Vector3 r2 = Dispersion(bb);
            Vector3 r3 = Alignment(bb) * alignment;
            Vector3 bound = BoundPosition(bb);
             //Vector3 tend = Tendacy(bb) * tendency;
            bb.velocity += (r1 + r2 + r3 + bound) / bb.mass;
            Limit(bb);

        }

        //center.position = SetCenter();
    }

    //Rule 1 Center of Mass
    public Vector3 Cohesion(BB b)
    {
        Vector3 pc = Vector3.zero;
        foreach (BB bj in boids)
        {
            if(bj != b)
            {
                pc += bj.transform.position; 
            }
        }

        pc = pc / (boids.Count - 1);

       return (pc - b.transform.position).normalized;
    }
    //Rule 2 Dispersion
    public Vector3 Dispersion(BB b)
    {
        Vector3 avoid = Vector3.zero;
        foreach(BB bj in boids)
        {
            if((bj.transform.position - b.transform.position).magnitude <= 20 * dispersion && bj != b)
            {
                avoid -= bj.transform.position - b.transform.position;
            }
        }
        return avoid.normalized;
    }

    public Vector3 Alignment(BB b)
    {
        Vector3 pv = Vector3.zero;
        foreach(BB bj in boids)
        {
            if (bj != b)
                pv += bj.velocity;
        }
        pv = pv / (boids.Count - 1);
        Vector3 rule3 = (pv - b.velocity).normalized;


        return rule3;

        
    }

    public Vector3 BoundPosition(BB b)
    {
        Vector3 BPos = new Vector3();

        if (b.transform.position.x > boundaries)
            BPos += new Vector3(-10, 0, 0);
        else if (b.transform.position.x < -boundaries)
            BPos += new Vector3(10, 0, 0);

        if (b.transform.position.y > boundaries)
            BPos += new Vector3(0, -10, 0);
        else if (b.transform.position.y < -boundaries)
            BPos += new Vector3(0, 10, 0);

        if (b.transform.position.z > boundaries)
            BPos += new Vector3(0, 0, -10);
        else if (b.transform.position.z < -boundaries)
            BPos += new Vector3(0, 0, 10);
        


        return BPos;
    }

    public Vector3 SetCenter()
    {
        Vector3 centermass = Vector3.zero;
        Vector3 positions = Vector3.zero;
        foreach (BB bb in boids)
        {
            positions += bb.transform.position;
        }

        centermass = positions / boids.Count;
        return centermass;
    }

    //public Vector3 Tendacy(BB bb)
    //{
    //    Vector3 place = transform.position;
    //    return (place - bb.transform.position/10).normalized;

    //}

    public void Limit(BB bb)
    {
        if (bb.velocity.magnitude > lim)
        {
            bb.velocity = (bb.velocity / (bb.velocity.magnitude) * lim).normalized;
        }
    }
}
