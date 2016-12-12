
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Cloth Simulation class
/// </summary>
public class ClothSim : MonoBehaviour
{
    [SerializeField]
    [Range(3f, 10)]
    
    private float Ks;
    [Range(3f, 10)]
    private float Kd;
    [Range(3f, 10)]
    private float Lo;

    private float gravity = 5f;

    /// <summary>
    /// Holds the height
    /// </summary>
    [Range(0.0f, 5f)]
    private int height;

    /// <summary>
    /// Holds the Width
    /// </summary>
    [Range(0.0f, 5)]
    private int width;

    /// <summary>
    /// Creates a float Slider
    /// </summary>
    [Range(0.0f, 5)]
    private float slider = 0;

    /// <summary>
    /// Creates a private float windz
    /// </summary>
    [Range(2f, 10f)]
    private float windz;

    /// <summary>
    /// Particles List
    /// </summary> 
    private List<Particle> particles;

    /// <summary>
    /// SpringDampers List
    /// </summary>
    private List<SpringDamper> springDampers;

    /// <summary>
    /// GameObjects List
    /// </summary>
    private List<GameObject> gameObjects;

    /// <summary>
    /// lineRenderers List
    /// </summary>
    private List<LineRenderer> lineRenderers;

    /// <summary>
    /// AeroDynamics List
    /// </summary>
    private List<Triangle> aeroDynamics;

    private bool Wind;

    /// <summary>
    /// Gameobject prefab
    /// </summary>
    private GameObject linePrefab;
   



   

    /// <summary>
    /// /Slider variables
    /// </summary>
    private Slider kS;
    private Slider kD;
    private Slider lO;
    private Slider wind;
    
    /////order would be to make a new Object()
    ////instantiate the gameObject that will represent it in the scene
    ////do the math to the Object in the game aka: update it
    ////update the gameObject with the new Object information
    
    /// <summary>
    /// Start function
    /// </summary>
    public void Start()
    {
        ////Renews each list
        this.particles = new List<Particle>();
        this.springDampers = new List<SpringDamper>();
        this.gameObjects = new List<GameObject>();
        this.aeroDynamics = new List<Triangle>();
        
////LineRenderer line = gameObject.AddComponent<LineRenderer>();


        for (int y = 0; y < this.height; ++y)
        {
            for(int x = 0; x < this.width; ++x)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                
                Particle p = new Particle(new Vector3(x * 5, -y, 0), Vector3.zero, 1f);
                go.transform.position = p.Position;
                this.gameObjects.Add(go);
                go.name = "Particle::" + (this.gameObjects.Count - 1).ToString();
                go.AddComponent<MonoParticle>();
                go.GetComponent<MonoParticle>().particle = p;
                this.particles.Add(p);
            }
        }

        for (int i = 0; i < this.particles.Count; ++i)
        {
            if(i % this.width != this.width - 1)
            {
                SpringDamper sdRight = new SpringDamper(this.particles[i], this.particles[i + 1], this.Ks, this.Kd, this.Lo);
                GameObject go = Instantiate(this.linePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdRight;
                this.springDampers.Add(sdRight);
            }
            if(i < (this.particles.Count - this.height))
            {
                SpringDamper sdDown = new SpringDamper(this.particles[i], this.particles[i + this.height], this.Ks, this.Kd, this.Lo);

                GameObject go = Instantiate(this.linePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdDown;
                this.springDampers.Add(sdDown);
            }
        }

        this.particles[0].Kinematic = true;
        this.particles[this.width - 1].Kinematic = true;

        for (int i = 0; i < this.width * this.height; i++)
        {
            if (i % this.width != this.width - 1)
            {
                if (i < (this.height * this.width) - this.width)
                {
                    Triangle t = new Triangle(this.particles[i], this.particles[i + 1], this.particles[i + this.width]);
                    this.aeroDynamics.Add(t);
                }
            }

            if (i % width != 0)
            {
                if (i < (this.height * this.width) - this.width)
                {
                    Triangle t = new Triangle(this.particles[i], this.particles[i + this.width], this.particles[i + this.width - 1]);
                    this.aeroDynamics.Add(t);
                }
            }
        }
       
    }
  
    /// <summary>
    /// update the user interface
    /// </summary>
    public void Update()
    {
        this.particles[0].Position = new Vector3(this.slider, 0, 0);
        foreach (SpringDamper sd in this.springDampers)
        { 
            ////Values of slider
            this.Ks = this.kS.value;
            this.Kd = this.kD.value;
            this.Lo = this.lO.value;
            ////windz = Wind.value;

            sd.Lo = this.Lo;
            sd.Ks = this.Ks;
            sd.Kd = this.Kd;
            
        } 
    }
    
    /// <summary>
    /// Fixed Update
    /// </summary>
    public void FixedUpdate()
    {
        foreach (Particle p in this.particles)
        {
            p.Force = Vector3.down * this.gravity * p.Mass;
           
        }
        ////foreach (Triangle t in aeroDynamics)
        ////{
        //    //aeroDynamics.Add(t);
        //    //t.Draw();
        ////}

        foreach (SpringDamper sd in this.springDampers)
        {
     
            sd.ComputeForce();
            //sd.Draw();
           
        }

        foreach (Triangle t in this.aeroDynamics)
        {
            t.ComputeAd(Vector3.forward * this.windz);
            if (this.wind)
            {
                if(!this.springDampers.Contains(t.D1) || !this.springDampers.Contains(t.D2) || !this.springDampers.Contains(t.D3))
                {
                    //this.aeroDynamics.Remove(t);
                }
                else
                {
                    t.ComputeAd(Vector3.forward * this.windz);
                    
                }
            }
        }


        foreach (Particle p in this.particles)
        {
            p.Update();
        }
    }

    /// <summary>
    /// LateUpdate class function
    /// </summary>
    public void LateUpdate()
    {
        //for(int i = 0; i < particles.Count; ++i)
        // {
        //     gameObjects[i].transform.position = particles[i].Position;
        // }
    }

    /// <summary>
    /// Function that reloads scene
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
        
    }
    
    /// <summary>
    /// Function that sets slider values
    /// </summary>
    public void SetSliders()
    {
       
        this.kS.value = 10f;
        this.kD.value = 10f;
        this.lO.value = 10f;
        this.wind.value = 10f;
      
        this.Ks = this.kS.value;
        this.Kd = this.kD.value;
        this.Lo = this.lO.value;
        this.windz = this.wind.value;
    }
}



