using UnityEngine;
using System.Collections;

public class MonoSpring : MonoBehaviour {

    public SpringDamper springDamper;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        GetComponent<LineRenderer>().SetPosition(0, springDamper.p1.Position);
        GetComponent<LineRenderer>().SetPosition(1, springDamper.p2.Position);

    }
}
