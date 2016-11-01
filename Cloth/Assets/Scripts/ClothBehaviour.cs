using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClothBehaviour : MonoBehaviour
{
    [Header("GameObjects")]
    static ClothBehaviour instance;

    private Vector3 Gravity = new Vector3(0, -0.5f, 0);
    
  

    public float limit; //limit velocity
    public Vector3 air;

    private float Drag;

    //Aero
    public float p;
    public float Cd;

    [SerializeField]
    [Range(1, 10)]
    public float ks;

    [SerializeField]
    [Range(1, 10)]
    public float kd;

    [SerializeField]
    [Range(1,5)]
    public float lo;

    public static ClothBehaviour self
    {
        get { return instance; }
    }

    [SerializeField]
    private SpringDamper[] springs;

    [SerializeField]
    private GameObject prefab;

    private List<Particles> particles;

    private int nParticles = 26;

    private int total;

    private LineRenderer line;

	// Use this for initialization
	void Start () {

        ks = Mathf.Clamp(ks, 1, 10);
        kd = Mathf.Clamp(kd, 1, 10);
        lo = Mathf.Clamp(lo, 1, 5);

        Drag = Mathf.Clamp(Drag, 1, 10);

        total = nParticles / 2;

        springs = new SpringDamper[total];

        for(int i = 0; i < total; i++)
        {
            //springs[i] = new SpringDamper(ks, kd);
        }
	
	}
    void Awake()
    {
        if (instance == null)
            instance = this;
    }
	
	// Update is called once per frame
	void Update () {
	    foreach (SpringDamper sd in springs)
        {
            sd.Lo = lo;
            sd.Ks = ks;
            sd.Kd = kd;
        }
	}
    void FixedUpdate()
    {
        foreach(Particles p in particles)
        {
            p.Force = Gravity * p.Mass;
        }
        foreach(SpringDamper sd in springs)
        {
            sd.ComputeForce();
        }
        foreach(Particles p in particles)
        {
            p.Update();
        }
    }
    void SpawnSpring(int sindex, int pindex , GameObject sphere)
    {
      
    }
    void SpawnNodes(int x, int y)
    {
       
    }
    
    void SpawnTriangles()
    {
       
    }

    void EulerInter(Node e)
    {//Euler Integration formula.
      if(!e.set)
        {
            e.accelleration = (e.force / e.mass);
            e.velocity += e.accelleration * Time.deltaTime;
            if (e.velocity.magnitude > limit)
            {
                e.velocity = e.velocity.normalized;
            }

            e.transform.position += e.velocity * Time.deltaTime;
            e.force = Vector3.zero;

        }   
    }
    void SpringForce(SpringDamper s)
    {

        
    }
    void AeroForce(AeroD a)
    {//Calculates the Wind on an object.
        Vector3 velocity = (a.p1.velocity + a.p2.velocity + a.p3.velocity) / 3;
        Vector3 relVelocity = velocity - air;

        Vector3 v = a.p2.transform.position - a.p1.transform.position;
        Vector3 w = a.p3.transform.position - a.p1.transform.position;
        Vector3 vNorm = v.normalized;
        Vector3 wNorm = w.normalized;
        Vector3 u = Vector3.Cross(vNorm, wNorm);

        Vector3 area = 0.5f * u;
        a.a = area * (Vector3.Dot(relVelocity, u));

        Vector3 aForce = -0.5f * p * (relVelocity.magnitude) * Cd * a.a;
        Vector3 sForce = aForce / 3;

        a.p1.force += sForce;
        a.p2.force += sForce;
        a.p3.force += sForce;
    }
}
