using UnityEngine;

/// <summary>
/// MonoParticle class
/// </summary>
public class MonoParticle : MonoBehaviour {

    /// <summary>
    /// Creates a public particle variable.
    /// </summary>
   public Particle particle;

    /// <summary>
    /// Start Function
    /// </summary>
    private void Start()
    {
        
    }


    /// <summary>
    /// Late Update
    /// </summary>
    private void LateUpdate()
    {
        this.transform.position = this.particle.Position;
    }

}
