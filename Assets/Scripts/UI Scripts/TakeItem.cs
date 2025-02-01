using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakeItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private InventoryManager im;
    public AudioSource audioSource;
    private ItemSlot itemSlot;
    private ItemDragManager idm;

    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();
        
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        idm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().itemDragContainer.GetComponent<ItemDragManager>(); 
    }

    public void Click()
    {
    
        audioSource.Play();
        im.TakeItem(itemSlot);
        idm.paused = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        idm.paused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        idm.paused = false;
    }    
}
