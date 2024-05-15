using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Vector2Int coord;
    public List<Item> items;
    private InventoryManager inventoryManager;

    void Start()
    {
        
        inventoryManager = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();
    }

    public void AddItems(List<Item> newItems)
    {

        items = newItems;
    }

    public void OpenContainer()
    {
        inventoryManager.ClearItemsLoot();
        inventoryManager.OpenLootPanel(items);
    }
}
