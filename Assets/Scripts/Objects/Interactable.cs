using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public ObjectLocation loc;

    public virtual bool Interact()
    {

        return true;
    }
}
