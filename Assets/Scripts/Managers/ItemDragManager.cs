using System.Collections;
using System.Collections.Generic;
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
        
        if(paused == false)
        {
            
            audioSource.PlayOneShot(itemSlot.item.contextClip);
            
            im.TransferToInventory(itemSlot, destinationItemSlot);
            this.itemSlot = null;      
            uiam.HideItemDrag();   
        }    
    }
}
