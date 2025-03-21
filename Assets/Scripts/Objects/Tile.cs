using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    [SerializeField] private IconType iconType = IconType.None;
    [SerializeField] private bool actionable = true; 
    
    public bool IsActionable()
    {

        return actionable;
    }

    public IconType GetIconType()
    {

        return iconType;
    }

    public void SetCoord(Vector2Int coord){
        
        this.coord = coord;
    }
}
