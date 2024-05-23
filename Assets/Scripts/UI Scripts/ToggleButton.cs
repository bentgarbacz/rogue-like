using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{

    public GameObject panel;
    private InventoryManager im;

    void Start()
    {
        im = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();
    }

    public void Click()
    {

        im.ToggleInventory();
    }

}
