using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClothSim : MonoBehaviour
{
    GameObject Sphere1, Sphere2;
    List<Particle> particles;
    List<SpringDamper> springDampers;
    List<GameObject> gameObjects;
    public List<LineRenderer> lineRenderers;
    public List<Triangle> aeroDynamics;
    public bool wind;

    public GameObject linePrefab;
   


    [SerializeField]
    [Range(2f, 10)]
    public float Ks;
    [Range(2f,10)]
    public float Kd;
    [Range(2f,10)]
    public float Lo;

    public float Gravity = 5f;

    [Range(0.0f, 5f)]
    public int height;

    [Range(0.0f, 5)]
    public int width;

    [Range(0.0f, 5)]
    public float slider = 0;

    [Range(2f, 10f)]
    public float windz;

    public Slider KS;
    public Slider KD;
    public Slider LO;
    public Slider Wind;
    
    //order would be to make a new Object()
    //instantiate the gameObject that will represent it in the scene
    //do the math to the Object in the game aka: update it
    //update the gameObject with the new Object information
    

    public void Start()
    {
        //Renews each list
        particles = new List<Particle>();
        springDampers = new List<SpringDamper>();
        gameObjects = new List<GameObject>();
        aeroDynamics = new List<Triangle>();
        
//        LineRenderer line = gameObject.AddComponent<LineRenderer>();


        for (int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                
                Particle p = new Particle(new Vector3(x * 5, -y, 0), Vector3.zero, 1f);
                go.transform.position = p.Position;
                gameObjects.Add(go);
                go.name = "Particle::" + (gameObjects.Count - 1).ToString();
                go.AddComponent<MonoParticle>();
                go.GetComponent<MonoParticle>().particle = p;
                particles.Add(p);
            }
        }

        for (int i = 0; i < particles.Count; ++i)
        {
            if(i % (width) != width -1)
            {
                SpringDamper sdRight = new SpringDamper(particles[i], particles[i + 1], Ks, Kd, Lo);
                GameObject go = Instantiate(linePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdRight;
                springDampers.Add(sdRight);
            }
            if(i < (particles.Count - height))
            {
                SpringDamper sdDown = new SpringDamper(particles[i], particles[i + height], Ks, Kd, Lo);

                GameObject go = Instantiate(linePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdDown;
                springDampers.Add(sdDown);
            }
        }

        particles[0].Kinematic = true;
        particles[width - 1].Kinematic = true;

        for (int i = 0; i < width * height; i++)
        {
            if (i % width != width - 1)
            {
                if (i < (height * width) - width)
                {
                    Triangle t = new Triangle(particles[i], particles[i + 1], particles[i + width]);
                    aeroDynamics.Add(t);
                }
            }

            if (i % width != 0)
            {
                if (i < (height * width) - width)
                {
                    Triangle t = new Triangle(particles[i], particles[i + width], particles[i + width - 1]);
                    aeroDynamics.Add(t);
                }
            }
        }
       
    }
  
    /// <summary>
    /// update the ui
    /// </summary>
    public void Update()
    {
        particles[0].Position = new Vector3(slider, 0, 0);
        foreach(SpringDamper sd in springDampers)
        { 
            Ks = KS.value;
            Kd = KD.value;
            Lo = LO.value;
            //windz = Wind.value;

            sd.Lo = Lo;
            sd.Ks = Ks;
            sd.Kd = Kd;
            
        } 
    }
    
    public void FixedUpdate()
    {
        foreach (Particle p in particles)
        {
            p.Force = Vector3.down * Gravity * p.Mass;
           
        }
        //foreach (Triangle t in aeroDynamics)
        //{
        //    //aeroDynamics.Add(t);
        //    //t.Draw();
        //}

        foreach (SpringDamper sd in springDampers)
        {
     
            sd.ComputeForce();
            //sd.Draw();
           
        }

        foreach (Triangle t in aeroDynamics)
        {
            t.ComputeAD(Vector3.forward * windz);
            if (wind)
            {
                if(!springDampers.Contains(t.D1) || !springDampers.Contains(t.D2) || !springDampers.Contains(t.D3))
                {
                    //aeroDynamics.Remove(t);
                }
                else
                {
                    t.ComputeAD(Vector3.forward * windz);
                    
                }
            }
        }


        foreach (Particle p in particles)
        {
            p.Update();
        }
    }

    public void LateUpdate()
    {
       //for(int i = 0; i < particles.Count; ++i)
       // {
       //     gameObjects[i].transform.position = particles[i].Position;
       // }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
        
    }

    public void SetSliders()
    {
       
        KS.value = 10f;
        KD.value = 10f;
        LO.value = 10f;
        Wind.value = 10f;

        
        Ks = KS.value;
        Kd = KD.value;
        Lo = LO.value;
        windz = Wind.value;
    }
}



