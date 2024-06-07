using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{

    private Camera mainCamera;
    private Vector3 min, max;
    private RectTransform tooltipContainerRect;
    public TextMeshProUGUI toolTipText;
    private UIActiveManager uiam;

    void Awake()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();

        mainCamera = Camera.main;
        tooltipContainerRect = GetComponent<RectTransform>();

        
        min = new Vector3(0, 0, 0);
        max = new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, 0);
    }

    void LateUpdate()
    {

        if (uiam.toolTipContainerIsOpen)
        {
            
            transform.position = new Vector3(
                                                Mathf.Clamp(Input.mousePosition.x + 10, min.x, max.x), 
                                                Mathf.Clamp(Input.mousePosition.y + tooltipContainerRect.rect.height, min.y, max.y), 
                                                transform.position.z
                                            );
        }
            
    }

    public void SetTooltip(bool status, string text = "")
    {

        toolTipText.SetText(text);
        toolTipText.ForceMeshUpdate(true);

        tooltipContainerRect.sizeDelta = toolTipText.GetRenderedValues() + new Vector2(10, 10);

        if(status)
        {

            uiam.ShowTooltip();

        }else if(!status)
        {

            uiam.HideTooltip();
        }
    }
}
