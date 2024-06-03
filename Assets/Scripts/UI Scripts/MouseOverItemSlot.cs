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
    private ToolTipManager ttm;

    void Start()
    {

        itemSlot = GetComponent<ItemSlot>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();

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

            if(transform.GetChild(0).GetComponent<InventoryContext>())
            {

                transform.GetChild(0).GetComponent<InventoryContext>().SetText();
            }
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
