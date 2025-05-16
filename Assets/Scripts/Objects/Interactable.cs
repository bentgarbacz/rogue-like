using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Vector2Int coord;

    public virtual bool Interact()
    {

        return true;
    }
}
