using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{


    private UIActiveManager uiam;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void Click()
    {

        uiam.ToggleInventory();
    }

}
