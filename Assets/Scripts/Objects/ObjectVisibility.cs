using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    public bool isActive = true;
    protected MeshRenderer meshRenderer;
    protected BoxCollider boxCollider;

    void Awake()
    {

        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        SetVisibility(false);
    }

    public void SetVisibility(bool state)
    {

        if (meshRenderer == null || boxCollider == null)
        {
            
            meshRenderer = GetComponent<MeshRenderer>();
            boxCollider = GetComponent<BoxCollider>();
        }

        meshRenderer.enabled = state;
        boxCollider.enabled = state;
    }
}
