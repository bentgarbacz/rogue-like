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
            
            if(Input.GetMouseButtonUp(0))
            {
                
                this.itemSlot = null;
                uiam.HideItemDrag();
            }
        }
            
    }
    
    public void DragItem(ItemSlot itemSlot)
    {

        if(paused == false)
        {
        
            this.itemSlot = itemSlot;
            itemDragImage.sprite = itemSlot.item.sprite;
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
            this.itemSlot = null;      
            uiam.HideItemDrag();   
        }    
    }

    //Determines if a drag transfer has a destination at an inventory slot
    public bool TransferToInventoryCheck(ItemSlot destinationItemSlot)
    {

        if(destinationItemSlot.type == "Inventory" || destinationItemSlot.type == "Drop" || destinationItemSlot.type == "Destroy")
        {

            return true;
        }

        return false;
    }

    //Checks if a transfer to a drop or destroy slot does not come from an invalid start slot type
    public bool ValidTransfertoDropOrDestroyCheck(ItemSlot destinationItemSlot)
    {

        if((itemSlot.type == "Loot" || im.equipmentSlotsDictionary.Keys.Contains(itemSlot.type)) && (destinationItemSlot.type == "Drop" || destinationItemSlot.type == "Destroy"))
        {

            return false;
        }

        return true;
    }
}
