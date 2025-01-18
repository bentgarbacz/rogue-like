using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemDragManager : MonoBehaviour
{

    public ItemSlot itemSlot = null;
    public Image itemDragImage;
    public bool paused = false;
    private RectTransform tooltipContainerRect;
    public AudioSource audioSource;
    private UIActiveManager uiam;
    private InventoryManager im;
    public bool isItemHeld = false;

    void Awake()
    {

        tooltipContainerRect = GetComponent<RectTransform>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();  
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
        }
            
    }
    
    public void DragItem(ItemSlot itemSlot)
    {

        if(paused == false)
        {
        
            this.itemSlot = itemSlot;
            itemDragImage.sprite = itemSlot.item.sprite;
            isItemHeld = true;
            uiam.ShowItemDrag();

            audioSource.PlayOneShot(itemSlot.item.contextClip);
        }
    }

    public void DropItem(ItemSlot destinationItemSlot)
    {
        
        if(paused == false && itemSlot.item != null)
        {
            
            audioSource.PlayOneShot(itemSlot.item.contextClip);
            
            im.TransferToInventory(itemSlot, destinationItemSlot);
            ForgetItem();  
        }    
    }

    public void ForgetItem()
    {

        this.itemSlot = null;
        isItemHeld = false;  
        uiam.HideItemDrag(); 
    }

    //Determines if a drag transfer has a destination at an inventory slot
    public bool TransferToInventoryCheck(ItemSlot destinationItemSlot)
    {

        if(destinationItemSlot.type is ItemSlotType.Inventory || destinationItemSlot.type is ItemSlotType.Drop || destinationItemSlot.type is ItemSlotType.Destroy)
        {

            return true;
        }

        return false;
    }

    //Checks if a transfer to a drop or destroy slot does not come from an invalid start slot type
    public bool ValidTransferToDropOrDestroyCheck(ItemSlot destinationItemSlot)
    {

        if((itemSlot.type is ItemSlotType.Loot || im.equipmentSlotsDictionary.Keys.Contains(itemSlot.type)) && (destinationItemSlot.type is ItemSlotType.Drop || destinationItemSlot.type is ItemSlotType.Destroy))
        {

            return false;
        }

        return true;
    }

    //Checks if items will be swapped between an equipment slot and an inventory slot
    public bool IsInverseEquip(ItemSlot destinationSlot)
    {
        
        bool destinationIsEmpty = destinationSlot.item == null;
        bool destinationIsInInventoryPanel = destinationSlot.type is ItemSlotType.Inventory || destinationSlot.type is ItemSlotType.Drop || destinationSlot.type is ItemSlotType.Destroy;
        bool dragSlotIsEquipment = im.equipmentSlotsDictionary.ContainsKey(itemSlot.type);

        if(dragSlotIsEquipment && destinationIsInInventoryPanel && !destinationIsEmpty)
        {

            return true;
        }

        return false;
    }

    public void OnDisable()
    {

        ForgetItem();
    }
}
