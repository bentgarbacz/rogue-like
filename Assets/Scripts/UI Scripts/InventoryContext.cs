using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryContext : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private InventoryManager im;
    private ItemSlot itemSlot;
    private ItemDragManager idm;
    public TextMeshProUGUI contextTextBox; 
    public AudioSource audioSource;
    public AudioClip errorClip;
    public AudioClip contextClip;
    

    void Awake()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        idm = GameObject.Find("System Managers").GetComponent<UIActiveManager>().itemDragContainer.GetComponent<ItemDragManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        errorClip = Resources.Load<AudioClip>("Sounds/Error");
    }

    public void Click()
    {
        
        contextClip = itemSlot.item.contextClip;

        if(im.ContextualAction(itemSlot))
        {

            audioSource.PlayOneShot(contextClip);

        }else
        {

            audioSource.PlayOneShot(errorClip);
        }
        
        idm.paused = true;
    }

    public void SetText()
    {

        contextTextBox.SetText(itemSlot.item.contextText);
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
