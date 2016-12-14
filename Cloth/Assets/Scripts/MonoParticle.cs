using System.Collections.Generic;
using System.Collections;
using UnityEngine;


/// <summary>
/// MonoParticle class
/// </summary>
public class MonoParticle : MonoBehaviour
{

    /// <summary>
    /// Creates a public particle variable.
    /// </summary>
   public Particle particle;

    /// <summary>
    /// Start Function
    /// </summary>
    public void Start()
    {
        
    }

   /// <summary>
   /// Late Update
   /// </summary>
   public void LateUpdate()
   {
        this.transform.position = this.particle.Position;
   }

}
