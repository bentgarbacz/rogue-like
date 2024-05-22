using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryContext : MonoBehaviour
{

    private InventoryManager im;
    private ItemSlot itemSlot;
    public TextMeshProUGUI contextTextBox; 

    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();
        im = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();        
    }

    public void Click()
    {
        
        im.ContextualAction(itemSlot);
    }

    public void SetText()
    {

        contextTextBox.SetText(itemSlot.item.contextText);

    }
}
