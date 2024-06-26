using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsMouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private UIActiveManager uiam;
    private bool isMouseOverSelf = false;

    void Awake()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        isMouseOverSelf = true;
        uiam.mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        isMouseOverSelf = false;
        uiam.mouseOverUI = false;
    }

    public void OnDisable()
    {

        if(isMouseOverSelf)
        {

            isMouseOverSelf = false;
            uiam.mouseOverUI = false;
        }
    }
}
