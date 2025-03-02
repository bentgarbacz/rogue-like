using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    [SerializeField] private bool actionable = true; 
    
    public bool IsActionable()
    {

        return actionable;
    }

    public void SetCoord(Vector2Int coord){
        
        this.coord = coord;
    }
}
