using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// MonoParticle class
/// </summary>
public class MonoParticle : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerUpHandler
{

    /// <summary>
    /// Creates a public particle variable.
    /// </summary>
   public Particle particle;

    private float speed;

    /// <summary>
    /// Start Function
    /// </summary>
    public void Start()
    {
        speed = 2;
        this.particle.Click = false;

    }

   /// <summary>
   /// Late Update
   /// </summary>
   public void LateUpdate()
   {
        this.transform.position = this.particle.Position;
   }

    public void OnDrag(PointerEventData eventData)
    {
        this.particle._position += new Vector3(eventData.delta.x, eventData.delta.y, particle._position.z) * Time.deltaTime * speed;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        particle.Click = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        particle.Click = false;
    }
}

