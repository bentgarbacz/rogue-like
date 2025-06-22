using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //public Vector2Int coord;
    public ObjectLocation loc;

    public virtual bool Interact()
    {

        return true;
    }
}
