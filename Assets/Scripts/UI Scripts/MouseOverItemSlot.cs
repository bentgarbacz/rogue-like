using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public List<GameObject> children = new();
    public bool state = false;
    public bool mouseOver = false;
    private ItemSlot itemSlot;
    private ToolTipManager ttm;
    private ItemDragManager idm;

    void Awake()
    {

        itemSlot = GetComponent<ItemSlot>();
        ttm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().toolTipContainer.GetComponent<ToolTipManager>();
        idm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().itemDragContainer.GetComponent<ItemDragManager>();

        foreach (Transform child in transform)
        {

            children.Add(child.gameObject);
        }
        
        SetChildren(state);
    }

    void Update()
    {

        if(mouseOver == true && itemSlot.item != null && Input.GetMouseButtonDown(0))
        {

            idm.DragItem(itemSlot);

        }else if(mouseOver == true && idm.itemSlot != null && Input.GetMouseButtonUp(0) && itemSlot.type != "Loot")
        {

            if(
               itemSlot.type == "Inventory" || 
               Rules.CheckValidEquipmentSlot(itemSlot, idm.itemSlot.item) && Rules.CheckValidEquipmentSlot(idm.itemSlot, itemSlot.item) ||
               Rules.CheckValidEquipmentSlot(itemSlot, idm.itemSlot.item) && itemSlot.item == null
              )

            {

                idm.DropItem(itemSlot);
            }            
        }
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
        
        mouseOver = true;
        MouseEnter();
    }

    public void MouseEnter()
    {

        if(state == false && itemSlot.item != null)
        {

            state = !state;
            SetChildren(state);

            ttm.SetTooltip(true, itemSlot.GetSummary());

            if(transform.GetChild(0).GetComponent<InventoryContext>())
            {

                transform.GetChild(0).GetComponent<InventoryContext>().SetText();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        mouseOver = false;
        ttm.SetTooltip(false);
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
