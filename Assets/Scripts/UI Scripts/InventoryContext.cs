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
    public AudioClip errorClip;
    public AudioClip contextClip;

    void Awake()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();

        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
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
    }

    public void SetText()
    {

        contextTextBox.SetText(itemSlot.item.contextText);
    }
}
