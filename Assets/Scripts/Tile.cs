using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    public bool traversable = true; 
    
    public void SetCoord(Vector2Int coord){
        
        this.coord = coord;
    }
}
