using UnityEngine;
using System.Collections;

namespace Inter
{
    public interface iMove
    {
        float mass { get; set; }
        Vector3 velocity { get; set; }
        Vector3 position { get; set; }
    }
}