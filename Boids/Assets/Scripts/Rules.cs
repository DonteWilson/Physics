using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rules : MonoBehaviour {

    /// <summary>
    /// Public list Boids
    /// </summary>
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

   /// <summary>
   /// Public float Cohesion
   /// </summary>
    [Range(0.0f, 5.00f)]
    public float cohesion;

    /// <summary>
    /// Public float dispersion
    /// </summary>
    [Range(0.0f, 5.00f)]
    public float dispersion;

    /// <summary>
    /// Public float alignment
    /// </summary>
    [Range(0.0f, 5.00f)]
    public float alignment;
    
    /// <summary>
    /// Public float tendency
    /// </summary>
    [Range(-1.0f,1.0f)]
    public float tendency;

    /// <summary>
    /// Public float slider
    /// </summary>
    [Range(0.0f, 5.00f)]
    public float slider = 0;


    /// <summary>
    /// Public float limit
    /// </summary>
    [Range(0.0f, 2.0f)]
    public float lim;


    /// <summary>
    /// /Slider variables
    /// </summary>
    public Slider slider_Cohe;
    public Slider slider_Align;
    public Slider slider_Separ;
    public Slider slider_Limit;


    /// <summary>
    /// Clamp Velocity Function
    /// </summary>
    /// <param name="vec">Vector Clamp</param>
    public void CV(Vector3 vec)
    {
        vec.x = Mathf.Clamp(vec.x, 0, 1);
        vec.y = Mathf.Clamp(vec.y, 0, 1);
        vec.z = Mathf.Clamp(vec.z, 0, 1);
    }


    /// <summary>
    /// Late Update Function
    /// </summary>
    public void LateUpdate()
    {
        foreach(BB bb in boids)
        {

            cohesion = slider_Cohe.value;
            dispersion = slider_Separ.value;
            alignment = slider_Align.value;

        }

    }

    /// <summary>
    /// Awake Function
    /// </summary>
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

    /// <summary>
    /// Fixed Update Function
    /// </summary>
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

    ////Rule 1 Center of Mass
   
    /// <summary>
    /// Vector 3 Cohesion
    /// </summary>
    /// <param name="b">boid list</param>
    /// <returns></returns>
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


    /// <summary>
    /// Dispersion Vector 3
    /// </summary>
    /// <param name="b">Boids in list</param>
    /// <returns> return avoid normalized</returns>
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


    /// <summary>
    /// Alignment Vector 3
    /// </summary>
    /// <param name="b">Boid in lsit</param>
    /// <returns>returns rule 3</returns>
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

    /// <summary>
    /// Sets the boundaries
    /// </summary>
    /// <param name="b">boids in list</param>
    /// <returns>returns boundaries position</returns>
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

    /// <summary>
    /// Sets the center of mass
    /// </summary>
    /// <returns>returns center of mass</returns>
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


    /// <summary>
    /// Limits the velocity
    /// </summary>
    /// <param name="bb">checks boid list</param>
    public void Limit(BB bb)
    {
        if (bb.velocity.magnitude > lim)
        {
            bb.velocity = (bb.velocity / (bb.velocity.magnitude) * lim).normalized;
        }
    }


    /// <summary>
    /// Reloads the Scene
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }


    /// <summary>
    /// Sets the sliders
    /// </summary>
    public void SetSliders()
    {
        slider_Cohe.value = 10f;
        slider_Align.value = 10f;
        slider_Separ.value = 10f;
        slider_Limit.value = 10f;

        cohesion = slider_Cohe.value;
        alignment = slider_Align.value;
        dispersion = slider_Separ.value;
        lim = slider_Limit.value;
    }
}
