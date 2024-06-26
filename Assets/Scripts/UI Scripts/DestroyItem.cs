using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{

    private AudioSource audioSource;
    private AudioClip destroyClip;
    private ItemSlot itemSlot;

    void Start()
    {

        itemSlot = transform.parent.gameObject.GetComponent<ItemSlot>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
        destroyClip = Resources.Load<AudioClip>("Sounds/DestroyItem");
    }

    public void Click()
    {

        audioSource.PlayOneShot(destroyClip);
        itemSlot.ThrowAway();
        GetComponentInParent<MouseOverItemSlot>().MouseExit();
        GetComponentInParent<MouseOverItemSlot>().MouseEnter();
    }
}
