using UnityEngine;
using System.Collections;

public class AeroD : MonoBehaviour {

    public Node p1, p2, p3;
    public Vector3 a;

    public void FormTriangle(Node a, Node b, Node c)
    {
        p1 = a;
        p2 = b;
        p3 = c;
    }
}
