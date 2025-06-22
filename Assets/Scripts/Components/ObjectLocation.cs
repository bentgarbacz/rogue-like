using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLocation : MonoBehaviour
{

    public Vector2Int coord;

    public Vector3 Coord3d()
    {

        return new Vector3((float)coord.x, 0f, (float)coord.y);
    }
}
