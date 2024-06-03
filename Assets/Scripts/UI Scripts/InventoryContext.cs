using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryContext : MonoBehaviour
{

    private InventoryManager im;
    private ItemSlot itemSlot;
    public TextMeshProUGUI contextTextBox; 
    public AudioSource audioSource;

    void Awake()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();    
    }

    public void Click()
    {
        
        audioSource.PlayOneShot(itemSlot.item.contextClip);
        im.ContextualAction(itemSlot);
    }

    public void SetText()
    {

        contextTextBox.SetText(itemSlot.item.contextText);
    }
}
