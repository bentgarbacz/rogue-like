using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int coord;
    public bool state = true;
    [SerializeField] private IconType iconType = IconType.None;
    [SerializeField] private bool actionable = true;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    void Awake()
    {

        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        SetState(false);
    }

    public bool IsActionable()
    {

        return actionable;
    }

    public IconType GetIconType()
    {

        return iconType;
    }

    public void SetCoord(Vector2Int coord)
    {

        this.coord = coord;
    }

    public void SetState(bool state)
    {

        this.state = state;

        if (state)
        {

            meshRenderer.enabled = true;
            boxCollider.enabled = true;

        }
        else
        {

            meshRenderer.enabled = false;


            if (actionable)
            {

                boxCollider.enabled = false;
            }
        }
    }
}
