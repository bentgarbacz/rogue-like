using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{

    [SerializeField] public Vector2Int coord;
    
    public void setCoord(Vector2Int crd){
        coord = crd;
    }
    
    public void ExitLevel()
    {


    }
}
