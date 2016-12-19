
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

    [Range(3f, 10f)]    
    public float Ks;
    [Range(3f, 10f)]
    public float Kd;
    [Range(3f, 10f)]
    public float Lo;

    public float Gravity = 5f;

    /// <summary>
    /// Creates a float Slider
    /// </summary>
    [Range(0.0f, 10)]
    public float Slider = 0;

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
    public Slider slider_KS;
    public Slider slider_KD;
    public Slider slider_LO;
    public Slider slider_WIND;

    /////order would be to make a new Object()
    ////instantiate the gameObject that will represent it in the scene
    ////do the math to the Object in the game aka: update it
    ////update the gameObject with the new Object information

    /// <summary>
    /// Start function
    /// </summary>
    public void Start()
    {
        //////LO.value = Lo;
        //////KD.value = Kd;
        //////KS.value = Ks;

        ////Renews each list
        Particles = new List<Particle>();
        SpringDampers = new List<SpringDamper>();
        GameObjects = new List<GameObject>();
        AeroDynamics = new List<Triangle>();
        
////LineRenderer line = gameObject.AddComponent<LineRenderer>();


        for (int y = 0; y < Height; ++y)
        {
            for (int x = 0; x < Width; ++x)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                
                Particle p = new Particle(new Vector3(x * 5, -y, 0), Vector3.zero, 1f);
                go.transform.position = p.Position;
                GameObjects.Add(go);
                go.name = "Particle::" + (GameObjects.Count - 1).ToString();
                go.AddComponent<MonoParticle>();
                go.GetComponent<MonoParticle>().particle = p;
                Particles.Add(p);
            }
        }

        for (int i = 0; i < Particles.Count; ++i)
        {
            if(i % Width != Width - 1)
            {
                SpringDamper sdRight = new SpringDamper(Particles[i], Particles[i + 1], Ks, Kd, Lo);
                GameObject go = Instantiate(LinePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdRight;
                SpringDampers.Add(sdRight);
            }
            if(i < (Particles.Count - Height))
            {
                SpringDamper sdDown = new SpringDamper(Particles[i], Particles[i + Height], Ks, Kd, Lo);

                GameObject go = Instantiate(LinePrefab) as GameObject;
                go.GetComponent<MonoSpring>().springDamper = sdDown;
                SpringDampers.Add(sdDown);
            }
        }

        Particles[0].Kinematic = true;
        Particles[Width - 1].Kinematic = true;

        for (int i = 0; i < Width * Height; i++)
        {
            if (i % Width != Width - 1)
            {
                if (i < (Height * Width) - Width)
                {
                    Triangle t = new Triangle(Particles[i], Particles[i + 1], Particles[i + Width]);
                    AeroDynamics.Add(t);
                }
            }

            if (i % Width != 0)
            {
                if (i < (Height * Width) - Width)
                {
                    Triangle t = new Triangle(Particles[i], Particles[i + Width], Particles[i + Width - 1]);
                    AeroDynamics.Add(t);
                }
            }
        }
       
    }
  
    /// <summary>
    /// update the user interface
    /// </summary>
    public void LateUpdate()
    {
        Particles[0].Position = new Vector3(Slider, 0, 0);
        foreach (SpringDamper sd in SpringDampers)
        {
            ////Values of slider
            Ks = slider_KS.value;
            Kd = slider_KD.value;
            Lo = slider_LO.value;
            windz = slider_WIND.value;
        }
    }
    
    /// <summary>
    /// Fixed Update
    /// </summary>
    public void FixedUpdate()
    {
        foreach (Particle p in Particles)
        {
            p.Force = Vector3.down * Gravity * p.Mass;
           
        }
        ////foreach (Triangle t in AeroDynamics)
        ////{
        //    //AeroDynamics.Add(t);
        //    //t.Draw();
        ////}

        foreach (SpringDamper sd in SpringDampers)
        {
     
            sd.ComputeForce();
            ////sd.Draw();

            if (sd.L > 30)
            {
                SpringDampers.Remove(sd);
                foreach (MonoSpring ms in FindObjectsOfType<MonoSpring>())
                {
                    if (ms.springDamper == sd)
                    {
                        ms.GetComponent<LineRenderer>().SetWidth(0, 0);
                        {
                            if (ms.springDamper.p1.Click)
                            {
                                this.Particles.Remove(ms.springDamper.p2);
                            }
                            if (ms.springDamper.p2.Click)
                            {
                                this.Particles.Remove(ms.springDamper.p2);
                            }
                            Destroy(ms.gameObject);
                        }
                    }
                }
            }

        }

        foreach (Triangle t in AeroDynamics)
        {
            t.ComputeAd(Vector3.forward * windz);
            if (slider_WIND)
            {
                if(!SpringDampers.Contains(t.D1) || !SpringDampers.Contains(t.D2) || !SpringDampers.Contains(t.D3))
                {
                    ////aeroDynamics.Remove(t);
                }
                else
                {
                    t.ComputeAd(Vector3.forward * windz);
                    
                }
            }
        }


        foreach (Particle p in Particles)
        {
            p.Update();
        }
    }

    ///// <summary>
    ///// LateUpdate class function
    ///// </summary>
    //public void LateUpdate()
    //{
    //    //////for (int i = 0; i < Particles.Count; ++i)
    //    //////{
    //    //////    GameObjects[i].transform.position = Particles[i].Position;
    //    //////}
    //}

    /// <summary>
    /// On Mouse Down Function
    /// </summary>
    public void OnMouseDown()
    {
        ScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(
                          Input.mousePosition.x,
                          Input.mousePosition.y,
                          ScreenPoint.z));

    }

    /// <summary>
    /// On Mouse Drag Button
    /// </summary>
    public void OnMouseDrag()
    {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenPoint.z);
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + Offset;
        transform.position = curPosition;

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

        slider_KS.value = 10f;
        slider_KD.value = 10f;
        slider_LO.value = 10f;
        slider_WIND.value = 10f;

        Ks = slider_KS.value;
        Kd = slider_KD.value;
        Lo = slider_LO.value;
        windz = slider_WIND.value;
    }
}



