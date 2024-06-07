using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color startcolor;
    public Color highlightColor = Color.yellow;
    public string actionDescription = "";
    private ToolTipManager ttm;

    void Start()
    {

        ttm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
        startcolor = GetComponent<Renderer>().material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if(actionDescription != "")
        {

            ttm.SetTooltip(true, actionDescription);
        }

        GetComponent<Renderer>().material.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ttm.SetTooltip(false);
        GetComponent<Renderer>().material.color = startcolor;
    }
}
