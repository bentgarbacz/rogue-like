using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tooltip = "";
    private ToolTipManager ttm;

    void Start()
    {
        
        ttm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
    }

    public void SetTooltip(string tooltip)
    {

        this.tooltip = tooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        ttm.SetTooltip(true, tooltip.Replace("\\n", "\n"));
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ClearTooltip();
    }

    public void OnDestroy()
    {
        
        ClearTooltip();
    }

    public void OnDisable()
    {

        ClearTooltip();   
    }

    public void ClearTooltip()
    {

        ttm.SetTooltip(false);
    }
}
