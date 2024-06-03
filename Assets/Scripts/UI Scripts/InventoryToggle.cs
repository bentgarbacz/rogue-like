using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{


    private UIActiveManager uiam;
    private AudioSource audioSource;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        audioSource = GameObject.Find("CanvasHUD").GetComponent<AudioSource>();
    }

    public void Click()
    {

        audioSource.Play();
        uiam.ToggleInventory();        
    }

}
