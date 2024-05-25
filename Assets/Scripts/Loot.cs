using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public Vector2Int coord;
    public List<Item> items;
    private UIActiveManager uiam;

    void Start()
    {
        
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
    }

    public void AddItems(List<Item> newItems)
    {

        items = newItems;
    }

    public void OpenContainer(GameObject container)
    {

        uiam.OpenLootPanel(items, container);
        
        uiam.OpenInventoryPanel();
    }
}
