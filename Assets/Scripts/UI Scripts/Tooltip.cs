using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tooltip = "";
    private ToolTipManager ttm;
    private bool currentToolTip = false;

    void Awake()
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
        currentToolTip = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ClearTooltip();
        currentToolTip = false;
    }

    public void OnDestroy()
    {

        if(currentToolTip == true)
        {
            Debug.Log("Destroy");
            Debug.Log(tooltip);

            ClearTooltip();   
        }
    }

    public void OnDisable()
    {

        if(currentToolTip == true)
        {

            Debug.Log("disable");
            Debug.Log(tooltip);
            ClearTooltip();   
        }
    }

    public void ClearTooltip()
    {

        ttm.SetTooltip(false);
    }
}
