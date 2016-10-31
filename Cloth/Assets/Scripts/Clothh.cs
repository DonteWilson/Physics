using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clothh : MonoBehaviour
{
    public Node nprefab;
    public List<Node> NODES = new List<Node>();
    public List<AeroD> Tri = new List<AeroD>();
    public float limit;


    public int Height;
    public int Width;

	// Use this for initialization
	void Start () {

        SpawnNodes(0, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SpawnNodes(int x, int y)
    {
        int pos = 1;
        for(int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Node Node = Instantiate(nprefab);
                Node.gameObject.name = "Node" + pos;
                pos++;
                Node.transform.parent = gameObject.transform;
                NODES.Add(Node);
            }
        }
    }

    void EulerInter(Node e)
    {
      if(!e.set)
        {
            e.accelleration = (e.force / e.mass);
            e.velocity += e.accelleration * Time.deltaTime;
            if (e.velocity.magnitude > limit)
            {
                e.velocity = e.velocity.normalized;
            }

            e.transform.position += e.velocity * Time.deltaTime;
            e.force = Vector3.zero;

        }   
    }
}
