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
        
        ttm = ttm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        ttm.SetTooltip(true, tooltip.Replace("\\n", "\n"));
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ttm.SetTooltip(false);
    }
}
