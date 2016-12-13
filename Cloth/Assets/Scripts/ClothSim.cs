
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Cloth Simulation class
/// </summary>

public class ClothSim : MonoBehaviour
{
    /// <summary>
    /// Holds the Height
    /// </summary>
    [Range(0.0f, 5f)]
    public int Height;

    /// <summary>
    /// Holds the Width
    /// </summary>
    [Range(0.0f, 5)]
    public int Width;

    /// <summary>
    /// Game object prefab
    /// </summary>
    public GameObject LinePrefab;


   

    [SerializeField]
    [Range(3f, 10f)]    
    public float Ks;
    [Range(3f, 10f)]
    public float Kd;
    [Range(3f, 10f)]
    public float Lo;

    public float gravity = 5f;

   

    /// <summary>
    /// Creates a float Slider
    /// </summary>
    [Range(0.0f, 5)]
    public float slider = 0;

    /// <summary>
    /// Creates a private float windz
    /// </summary>
    [Range(2f, 10f)]
    public float windz;

    /// <summary>
    /// Particles List
    /// </summary> 
    public List<Particle> Particles;

    /// <summary>
    /// SpringDampers List
    /// </summary>
    public List<SpringDamper> SpringDampers;

    /// <summary>
    /// GameObjects List
    /// </summary>
    public List<GameObject> GameObjects;

    /// <summary>
    /// lineRenderers List
    /// </summary>
    public List<LineRenderer> LineRenderers;

    /// <summary>
    /// AeroDynamics List
    /// </summary>
    public List<Triangle> AeroDynamics;

    public bool Wind;


    public Vector3 ScreenPoint;

    public Vector3 Offset;

    /// <summary>
    /// /Slider variables
    /// </summary>
    public Slider KS;
    public Slider KD;
    public Slider LO;
    public Slider wind;

    /////order would be to make a new Object()
    ////instantiate the gameObject that will represent it in the scene
    ////do the math to the Object in the game aka: update it
    ////update the gameObject with the new Object information

    /// <summary>
    /// Start function
    /// </summary>
    public void Start()
    {
        this.LO.value = this.Lo;
        this.KD.value = this.Kd;
        this.KS.value = this.Ks;

        ////Renews each list
        this.Particles = new List<Particle>();
        this.SpringDampers = new List<SpringDamper>();
        this.GameObjects = new List<GameObject>();
        this.AeroDynamics = new List<Triangle>();
        
////LineRenderer line = gameObject.AddComponent<LineRenderer>();


        for (int y = 0; y < this.Height; ++y)
        {
            for(int x = 0; x < this.Width; ++x)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                
                Particle p = new Particle(new Vector3(x * 5, -y, 0), Vector3.zero, 1f);
                go.transform.position = p.Position;
                this.GameObjects.Add(go);
                go.name = "Particle::" + (this.GameObjects.Count - 1).ToString();
                go.AddComponent<MonoParticle>();
                go.GetComponent<MonoParticle>().particle = p;
                this.Particles.Add(p);
            }
        }

        for (int i = 0; i < this.Particles.Count; ++i)
        {
            if(i % this.Width != this.Width - 1)
            {
                SpringDamper sdRight = new SpringDamper(this.Particles[i], this.Particles[i + 1], this.Ks, this.Kd, this.Lo);
                GameObject go = Instantiate(this.LinePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdRight;
                this.SpringDampers.Add(sdRight);
            }
            if(i < (this.Particles.Count - this.Height))
            {
                SpringDamper sdDown = new SpringDamper(this.Particles[i], this.Particles[i + this.Height], this.Ks, this.Kd, this.Lo);

                GameObject go = Instantiate(this.LinePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdDown;
                this.SpringDampers.Add(sdDown);
            }
        }

        this.Particles[0].Kinematic = true;
        this.Particles[this.Width - 1].Kinematic = true;

        for (int i = 0; i < this.Width * this.Height; i++)
        {
            if (i % this.Width != this.Width - 1)
            {
                if (i < (this.Height * this.Width) - this.Width)
                {
                    Triangle t = new Triangle(this.Particles[i], this.Particles[i + 1], this.Particles[i + this.Width]);
                    this.AeroDynamics.Add(t);
                }
            }

            if (i % this.Width != 0)
            {
                if (i < (this.Height * this.Width) - this.Width)
                {
                    Triangle t = new Triangle(this.Particles[i], this.Particles[i + this.Width], this.Particles[i + this.Width - 1]);
                    this.AeroDynamics.Add(t);
                }
            }
        }
       
    }
  
    /// <summary>
    /// update the user interface
    /// </summary>
    public void Update()
    {
        this.Particles[0].Position = new Vector3(this.slider, 0, 0);
        foreach (SpringDamper sd in this.SpringDampers)
        { 
            ////Values of slider
            this.Ks = this.KS.value;
            this.Kd = this.KD.value;
            this.Lo = this.LO.value;
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
        foreach (Particle p in this.Particles)
        {
            p.Force = Vector3.down * this.gravity * p.Mass;
           
        }
        ////foreach (Triangle t in AeroDynamics)
        ////{
        //    //AeroDynamics.Add(t);
        //    //t.Draw();
        ////}

        foreach (SpringDamper sd in this.SpringDampers)
        {
     
            sd.ComputeForce();
            ////sd.Draw();
           
        }

        foreach (Triangle t in this.AeroDynamics)
        {
            t.ComputeAd(Vector3.forward * this.windz);
            if (this.wind)
            {
                if(!this.SpringDampers.Contains(t.D1) || !this.SpringDampers.Contains(t.D2) || !this.SpringDampers.Contains(t.D3))
                {
                    ////this.aeroDynamics.Remove(t);
                }
                else
                {
                    t.ComputeAd(Vector3.forward * this.windz);
                    
                }
            }
        }


        foreach (Particle p in this.Particles)
        {
            p.Update();
        }
    }

    /// <summary>
    /// LateUpdate class function
    /// </summary>
    public void LateUpdate()
    {
        ////for(int i = 0; i < Particles.Count; ++i)
        // {
        //     GameObjects[i].transform.position = Particles[i].Position;
        // }
    }

    /// <summary>
    /// On Mouse Down Function
    /// </summary>
    public void OnMouseDown()
    {
        this.ScreenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        this.Offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(
                          Input.mousePosition.x,
                          Input.mousePosition.y,
                          this.ScreenPoint.z));

    }

    public void OnMouseDrag()
    {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.ScreenPoint.z);
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + this.Offset;
        this.transform.position = curPosition;

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
       
        this.KS.value = 10f;
        this.KD.value = 10f;
        this.LO.value = 10f;
        this.wind.value = 10f;
      
        this.Ks = this.KS.value;
        this.Kd = this.KD.value;
        this.Lo = this.LO.value;
        this.windz = this.wind.value;
    }
}



