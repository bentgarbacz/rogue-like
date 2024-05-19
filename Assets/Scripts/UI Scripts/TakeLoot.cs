using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeLoot : MonoBehaviour
{

    private InventoryManager im;
    private ItemSlot itemSlot;

    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();
        im = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();        
    }

    public void Click()
    {
        
        im.TakeLoot(itemSlot);
    }
    
}
