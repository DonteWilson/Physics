using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {

    GameObject current = null;

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
    
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(ShootRay() != null && ShootRay().GetComponent<Particles>() != null)
            {
                current = ShootRay();
               
            }

            else
            {
                current = null;
            }
        }
    }

    public void LateUpdate()
    {
        if(Input.GetMouseButtonDown(1) && ShootRay().GetComponent<Particles>() != null)
        {
            current = ShootRay();
        }
        if (Input.GetMouseButton(1) && current != null)
        {
            current.GetComponent<Particles>().Force = Vector3.zero;
            current.GetComponent<Particles>().Velocity = Vector3.zero;

            Vector3 mouse = Input.mousePosition;
            mouse.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
            worldPos.z = current.transform.position.z;
            current.GetComponent<Particles>().Position = worldPos;
            current.transform.position = worldPos;
        }
    }
}
