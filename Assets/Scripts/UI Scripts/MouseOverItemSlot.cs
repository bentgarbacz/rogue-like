using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<GameObject> children = new List<GameObject>();
    public bool state = false;
    private ItemSlot itemSlot;
    private InventoryManager im;

    void Start()
    {

        itemSlot = GetComponent<ItemSlot>();
        im = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();

        foreach (Transform child in transform)
        {

            children.Add(child.gameObject);
        }
        
        SetChildren(state);
    }

    private void SetChildren(bool newState)
    {

        foreach (GameObject child in children)
        {

            child.SetActive(newState);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        MouseEnter();
    }

    public void MouseEnter()
    {

        if(state == false && itemSlot.item != null)
        {

            state = !state;
            SetChildren(state);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExit();
    }

    public void MouseExit()
    {

        if(state == true)
        {
            
            state = !state;
            SetChildren(state);
        }
    }
}
