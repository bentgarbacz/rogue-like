using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsMouseOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private UIActiveManager uiam;

    void Awake()
    {
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        uiam.mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiam.mouseOverUI = false;
    }

    public void OnDisable()
    {

        uiam.mouseOverUI = false;
    }
}
