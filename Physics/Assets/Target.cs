using UnityEngine;

/// <summary>
/// Creates class for Target
/// </summary>
public class Target : MonoBehaviour
{

    /// <summary>
    /// Contains private integer for agent
    /// </summary>
    protected int agent;

    /// <summary>
    /// Contains private integer for max distance
    /// </summary>
    protected int mDist;

    /// <summary>
    /// private gameobject prefab
    /// </summary>
    public GameObject prefab;


    /// <summary>
    /// Calls function in awake
    /// </summary>
    private void Awake()
    {
        for(int i = 0; i < this.agent; i++)
        {
            Vector3 pos = new Vector3();
            pos.x = Random.Range(-this.mDist, this.mDist);
            pos.y = Random.Range(-this.mDist, this.mDist);
            pos.z = Random.Range(-this.mDist, this.mDist);

            GameObject temp = Instantiate(this.prefab,pos, Quaternion.identity) as GameObject;
            Agent a = temp.GetComponent<Agent>();
            a.target = this.gameObject.transform;
            a.mass = Random.Range(5, 20);
        }
    }
}
