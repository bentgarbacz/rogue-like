using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibility : MonoBehaviour
{
    public bool isActive = true;
    protected MeshRenderer meshRenderer;
    protected BoxCollider boxCollider;
    protected MapIconController mic = null;

    public void Initialize(bool initialState = false)
    {

        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        SetVisibility(initialState);
    }

    public void SetIcon(MapIconController mic)
    {

        this.mic = mic;
    }

    public void SetVisibility(bool state)
    {

        meshRenderer.enabled = state;
        boxCollider.enabled = state;

        if (mic != null)
        {

            mic.SetVisibility(state);
        }
    }
}
