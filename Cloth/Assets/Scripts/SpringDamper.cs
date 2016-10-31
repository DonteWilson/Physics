using UnityEngine;
using System.Collections;

public class SpringDamper : MonoBehaviour
{
    public float RestLength; //rest length

    public Node p1; //node1
    public Node p2; //node 2

    public LineRenderer spring;

   
    //Spawn Springs function in Cloth Behaviour class
    public void MakeSpring(Node a, Node b, float dis)
    {
        p1 = a;
        p2 = b;
        RestLength = dis;
        DrawLines();
    }

    //Draws the lines between nodes the spring is connected to
    public void DrawLines()
    {
        spring.SetPosition(0, p1.transform.position);
        spring.SetPosition(1, p2.transform.position);
    }
}