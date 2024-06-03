using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeItem : MonoBehaviour
{

    private InventoryManager im;
    public AudioSource audioSource;
    private ItemSlot itemSlot;

    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>(); 
    }

    public void Click()
    {
    
        audioSource.Play();
        im.TakeItem(itemSlot);
    }    
}
