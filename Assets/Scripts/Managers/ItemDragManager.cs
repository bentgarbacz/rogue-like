using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragManager : MonoBehaviour
{

    public ItemSlot itemSlot = null;
    public Image itemDragImage;
    private RectTransform tooltipContainerRect;
    private UIActiveManager uiam;
    private InventoryManager im;

    void Awake()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        tooltipContainerRect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {

        if (uiam.itemDragContainerIsOpen)
        {
            
            transform.position = new Vector3(
                                                Input.mousePosition.x - 10, 
                                                Input.mousePosition.y - tooltipContainerRect.rect.height,
                                                transform.position.z
                                            );
            
            if(Input.GetMouseButtonUp(0))
            {
                
                this.itemSlot = null;
                uiam.HideItemDrag();
            }
        }
            
    }

    public void DragItem(ItemSlot itemSlot)
    {

        this.itemSlot = itemSlot;
        itemDragImage.sprite = itemSlot.item.sprite;
        uiam.ShowItemDrag();
    }

    public void DropItem(ItemSlot destinationItemSlot)
    {
        im.TransferToInventory(itemSlot, destinationItemSlot);
        destinationItemSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();

        this.itemSlot = null;      
        uiam.HideItemDrag();
    }
}
