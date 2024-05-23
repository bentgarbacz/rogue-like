using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Vector2Int coord;
    public List<Item> items;
    private InventoryManager im;

    void Start()
    {
        
        im = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();
    }

    public void AddItems(List<Item> newItems)
    {

        items = newItems;
    }

    public void OpenContainer(GameObject container)
    {

        im.OpenLootPanel(items, container);
        im.OpenInventoryPanel();
    }
}
