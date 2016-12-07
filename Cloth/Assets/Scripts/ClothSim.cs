using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClothSim : MonoBehaviour
{
    
    List<Particle> _particles;
    List<SpringDamper> _springDampers;
    List<GameObject> _gameObjects;
    public List<LineRenderer> LineRenderers;
    public List<Triangle> AeroDynamics;
    public bool Wind;

    public GameObject LinePrefab;
   


    [SerializeField]
    [Range(3f, 10)]
    public float Ks;
    [Range(3f,10)]
    public float Kd;
    [Range(3f,10)]
    public float Lo;

    public float Gravity = 5f;

    [Range(0.0f, 5f)]
    public int Height;

    [Range(0.0f, 5)]
    public int Width;

    [Range(0.0f, 5)]
    public float Slider = 0;

    [Range(2f, 10f)]
    public float Windz;

    public Slider KS;
    public Slider KD;
    public Slider LO;
    public Slider wind;
    
    //order would be to make a new Object()
    //instantiate the gameObject that will represent it in the scene
    //do the math to the Object in the game aka: update it
    //update the gameObject with the new Object information
    

    public void Start()
    {
        //Renews each list
        _particles = new List<Particle>();
        _springDampers = new List<SpringDamper>();
        _gameObjects = new List<GameObject>();
        AeroDynamics = new List<Triangle>();
        
//        LineRenderer line = gameObject.AddComponent<LineRenderer>();


        for (int y = 0; y <Height; ++y)
        {
            for(int x = 0; x < Width; ++x)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                
                Particle p = new Particle(new Vector3(x * 5, -y, 0), Vector3.zero, 1f);
                go.transform.position = p.Position;
                _gameObjects.Add(go);
                go.name = "Particle::" + (_gameObjects.Count - 1).ToString();
                go.AddComponent<MonoParticle>();
                go.GetComponent<MonoParticle>().particle = p;
                _particles.Add(p);
            }
        }

        for (int i = 0; i < _particles.Count; ++i)
        {
            if(i % (Width) != Width -1)
            {
                SpringDamper sdRight = new SpringDamper(_particles[i], _particles[i + 1], Ks, Kd, Lo);
                GameObject go = Instantiate(LinePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdRight;
                _springDampers.Add(sdRight);
            }
            if(i < (_particles.Count -Height))
            {
                SpringDamper sdDown = new SpringDamper(_particles[i], _particles[i +Height], Ks, Kd, Lo);

                GameObject go = Instantiate(LinePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdDown;
                _springDampers.Add(sdDown);
            }
        }

        _particles[0].Kinematic = true;
        _particles[Width - 1].Kinematic = true;

        for (int i = 0; i < Width *Height; i++)
        {
            if (i % Width != Width - 1)
            {
                if (i < (Height * Width) - Width)
                {
                    Triangle t = new Triangle(_particles[i], _particles[i + 1], _particles[i + Width]);
                    AeroDynamics.Add(t);
                }
            }

            if (i % Width != 0)
            {
                if (i < (Height * Width) - Width)
                {
                    Triangle t = new Triangle(_particles[i], _particles[i + Width], _particles[i + Width - 1]);
                    AeroDynamics.Add(t);
                }
            }
        }
       
    }
  
    /// <summary>
    /// update the ui
    /// </summary>
    public void Update()
    {
        _particles[0].Position = new Vector3(Slider, 0, 0);
        foreach(SpringDamper sd in _springDampers)
        { 
            //Values of slider
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
        foreach (Particle p in _particles)
        {
            p.Force = Vector3.down * Gravity * p.Mass;
           
        }
        //foreach (Triangle t in AeroDynamics)
        //{
        //    //AeroDynamics.Add(t);
        //    //t.Draw();
        //}

        foreach (SpringDamper sd in _springDampers)
        {
     
            sd.ComputeForce();
            //sd.Draw();
           
        }

        foreach (Triangle t in AeroDynamics)
        {
            t.ComputeAD(Vector3.forward * Windz);
            if (wind)
            {
                if(!_springDampers.Contains(t.D1) || !_springDampers.Contains(t.D2) || !_springDampers.Contains(t.D3))
                {
                    //AeroDynamics.Remove(t);
                }
                else
                {
                    t.ComputeAD(Vector3.forward * Windz);
                    
                }
            }
        }


        foreach (Particle p in _particles)
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
        wind.value = 10f;

        
        Ks = KS.value;
        Kd = KD.value;
        Lo = LO.value;
        Windz = wind.value;
    }
}



