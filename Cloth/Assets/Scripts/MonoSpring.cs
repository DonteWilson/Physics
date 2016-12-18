using UnityEngine;


public class MonoSpring : MonoBehaviour {

    /// <summary>
    /// Sets a public spring damper
    /// </summary>
    public SpringDamper springDamper;
    //// Use this for initialization


    /// <summary>
    /// Start Function
    /// </summary>
    public void Start()
    {
        
    }
    //// Update is called once per frame

        /// <summary>
        /// Late Update Function
        /// </summary>
    public void LateUpdate()
    {
        this.GetComponent<LineRenderer>().SetPosition(0, this.springDamper.p1.Position);
        this.GetComponent<LineRenderer>().SetPosition(1, this.springDamper.p2.Position);
    }
}
