using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public Vector2Int coord;
    [SerializeField] public bool traversible;
    
    public void setCoord(Vector2Int crd){
        coord = crd;
    }
}
