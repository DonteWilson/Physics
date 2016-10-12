using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

    public int agent;
    public int mDist;
    public GameObject prefab;

    void Awake()
    {
        for(int i = 0; i < agent; i++)
        {
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-mDist, mDist);
            pos.y = Random.Range(-mDist, mDist);
            pos.z = Random.Range(-mDist, mDist);

            GameObject temp = Instantiate(prefab,pos, Quaternion.identity) as GameObject;
            Agent a = temp.GetComponent<Agent>();
            a.target = gameObject.transform;
            a.mass = Random.Range(5, 20);
        }
    }
}
