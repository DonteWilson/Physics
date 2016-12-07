using UnityEngine;
using System.Collections;
//Need to fix some portions of code, so that I can grab the game object correctly.
//Currently having issues 
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
            if(ShootRay() != null && ShootRay().GetComponent<GameObject>() != null)
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
        if(Input.GetMouseButtonDown(1) && ShootRay().GetComponent<GameObject>() != null)
        {
            current = ShootRay();
        }
        if (Input.GetMouseButton(1) && current != null)
        {
            current.GetComponent<Particle>().Force = Vector3.zero;
            current.GetComponent<Particle>().Velocity = Vector3.zero;

            Vector3 mouse = Input.mousePosition;
            mouse.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
            worldPos.z = current.transform.position.z;
            current.GetComponent<Particle>().Position = worldPos;
            current.transform.position = worldPos;
        }
    }
}
