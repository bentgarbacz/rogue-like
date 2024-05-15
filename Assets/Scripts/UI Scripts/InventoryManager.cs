using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> lootSlots = new List<ItemSlot>();
    public GameObject inventoryPanel;
    public GameObject lootPanel;

    void Start()
    {

        inventoryPanel = GameObject.Find("CanvasHUD").GetComponent<InactiveReferences>().InventoryPanel;
        lootPanel = GameObject.Find("CanvasHUD").GetComponent<InactiveReferences>().LootPanel;

        GameObject invGrid = inventoryPanel.transform.GetChild(0).gameObject;
        GameObject lootGrid = lootPanel.transform.GetChild(0).gameObject;

        inventorySlots.Add(invGrid.transform.GetChild(0).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(1).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(2).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(3).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(4).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(5).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(6).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(7).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(8).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(9).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(10).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(11).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(12).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(13).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(14).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(15).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(16).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(17).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(18).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(19).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(20).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(21).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(22).gameObject.GetComponent<ItemSlot>());
        inventorySlots.Add(invGrid.transform.GetChild(23).gameObject.GetComponent<ItemSlot>());

        lootSlots.Add(lootGrid.transform.GetChild(0).gameObject.GetComponent<ItemSlot>());
        lootSlots.Add(lootGrid.transform.GetChild(1).gameObject.GetComponent<ItemSlot>());
        lootSlots.Add(lootGrid.transform.GetChild(2).gameObject.GetComponent<ItemSlot>());
        lootSlots.Add(lootGrid.transform.GetChild(3).gameObject.GetComponent<ItemSlot>());
        lootSlots.Add(lootGrid.transform.GetChild(4).gameObject.GetComponent<ItemSlot>());
        lootSlots.Add(lootGrid.transform.GetChild(5).gameObject.GetComponent<ItemSlot>());
        lootSlots.Add(lootGrid.transform.GetChild(6).gameObject.GetComponent<ItemSlot>());
    }

    public bool AddItemInventory(Item invItem)
    {

        foreach(ItemSlot slot in inventorySlots)
        {

            if(slot.item == null)
            {

                slot.AddItem(invItem);
                return true;
            }
        }

        return false;
    }

    public void OpenInventoryPanel()
    {

        if(inventoryPanel){
            
            inventoryPanel.GetComponent<ToggleButton>().Click();
        }
    }

    public bool AddItemLoot(Item lootItem)
    {
        
        foreach(ItemSlot slot in lootSlots)
        {

            if(slot.item == null)
            {
                print("item manager");
                slot.AddItem(lootItem);
                return true;
            }
        }

        return false;
    }

    public void ClearItemsLoot()
    {
        
        foreach(ItemSlot slot in lootSlots)
        {

            if(slot.item != null)
            {

                slot.ThrowAway();
            }
        }
    }

    public void OpenLootPanel(List<Item> items)
    {

        foreach(Item i in items)
        {

            AddItemLoot(i);
        }

        if(lootPanel){
            
            lootPanel.GetComponent<ToggleButton>().Click();
        }
    }
}
