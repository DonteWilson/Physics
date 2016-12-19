using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Target : MonoBehaviour
{

    public int agent;
    public int mDist;
    public GameObject prefab;
    public int minM;
    public int maxM;
    public float radius;
    [Range(.1f, 1.5f)]public float steeringB;

    public void Awake()
    {
        for (int i = 0; i < agent; i++)
        {
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-mDist, mDist);
            pos.y = Random.Range(-mDist, mDist);
            pos.z = Random.Range(-mDist, mDist);

            GameObject temp = Instantiate(prefab, pos, new Quaternion()) as GameObject;

            if(temp.GetComponent<SB>() != null)
            {
                SB sb = temp.GetComponent<SB>();
                sb.target = gameObject.transform;
                sb.steeringF = steeringB;
            }
            if(temp.GetComponent<SA>() !=null)
            {
                SA sa = temp.GetComponent<SA>();
                sa.target = gameObject.transform;
                sa.steeringF = steeringB;
                sa.radius = radius;
            }
            MonoB m = temp.GetComponent<MonoB>();
            m.boid.position = pos;
            m.mass = Random.Range(minM, maxM);
            m.boid.velocity = Vector3.up;
        
        }
    }

    public void Update()
    {
        foreach(SA sa in FindObjectsOfType<SA>())
        {
            sa.target = gameObject.transform;
            sa.steeringF = steeringB;
            sa.radius = radius;
        }
        foreach(SB sa in FindObjectsOfType<SB>())
        {
            sa.target = gameObject.transform;
            sa.steeringF = steeringB;
        }
    }  
}
