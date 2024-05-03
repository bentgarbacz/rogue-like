using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    private Color startcolor;
    public Color outlineColor = Color.yellow;

    void Start()
    {

        startcolor = GetComponent<Renderer>().material.color;
        //startcolor = Color.clear;
    }

    void OnMouseEnter()
    {
        
        GetComponent<Renderer>().material.color = outlineColor;
    }

    void OnMouseExit()
    {

        GetComponent<Renderer>().material.color = startcolor;
    }
}
