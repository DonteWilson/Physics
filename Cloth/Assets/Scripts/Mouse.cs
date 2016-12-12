using UnityEngine;

////Need to fix some portions of code, so that I can grab the game object correctly.
////Currently having issues 
public class Mouse : MonoBehaviour {

    /// <summary>
    /// Sets current game object to null
    /// </summary>
    private GameObject current = null;

    /// <summary>
    /// Game Object ShootRay
    /// </summary>
    /// <returns>hit game object</returns>
    public GameObject ShootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray.origin, ray.direction, out hit);
        if(hit.transform != null)

        {
            return hit.transform.gameObject;
        }
        return null;
    }

    /// <summary>
    /// Update function
    /// </summary>
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(this.ShootRay() != null && this.ShootRay().GetComponent<GameObject>() != null)
            {
                this.current = this.ShootRay();            
            }
           else
            {
                this.current = null;
            }
        }
    }

    /// <summary>
    /// Late Update
    /// </summary>
    public void LateUpdate()
    {
        if(Input.GetMouseButtonDown(1) && this.ShootRay().GetComponent<GameObject>() != null)
        {
            this.current = this.ShootRay();
        }
        if (Input.GetMouseButton(1) && this.current != null)
        {
            this.current.GetComponent<Particle>().Force = Vector3.zero;
            this.current.GetComponent<Particle>().Velocity = Vector3.zero;

            Vector3 mouse = Input.mousePosition;
            mouse.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
            worldPos.z = this.current.transform.position.z;
            this.current.GetComponent<Particle>().Position = worldPos;
            this.current.transform.position = worldPos;
        }
    }
}
