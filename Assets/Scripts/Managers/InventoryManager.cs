using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> lootSlots = new List<ItemSlot>();
    private readonly int inventorySlotCount = 24;
    private readonly int lootSlotCount = 8;
    public GameObject currentLootContainer;
    private UIActiveManager uiam;

    void Start()
    {

        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();

        GameObject invGrid = uiam.inventoryPanel.transform.GetChild(0).gameObject;
        GameObject lootGrid = uiam.lootPanel.transform.GetChild(0).gameObject;

        ConstructItemSlotList(inventorySlots, invGrid, inventorySlotCount);
        ConstructItemSlotList(lootSlots, lootGrid, lootSlotCount);    
    }

    private void ConstructItemSlotList(List<ItemSlot> slotList, GameObject grid, int children)
    {

        for(int i = 0; i < children; i++)
        {

            ItemSlot slot = grid.transform.GetChild(i).gameObject.GetComponent<ItemSlot>();

            slot.slotIndex = i; 
            slotList.Add(slot);
        }
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

    public bool AddItemLoot(Item lootItem)
    {
        
        foreach(ItemSlot slot in lootSlots)
        {

            if(slot.item == null)
            {

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

            slot.ThrowAway();
        }
    }

    public void TakeLoot(ItemSlot targetSlot)
    {

        foreach(ItemSlot i in inventorySlots)
        {

            if(i.item == null)
            {

               List<Item> holdItemList = targetSlot.itemList;

               targetSlot.TransferItem(i);
               
               targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();               
               uiam.CloseLootPanel();
               uiam.OpenLootPanel(holdItemList, currentLootContainer);
               targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();

               break; 
            }
        }
    }

    public void ContextualAction(ItemSlot targetSlot)
    {

        targetSlot.item.Use();

        if(targetSlot.item is Consumable)
        {
            
            targetSlot.ThrowAway();
            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();
            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();
        }
    }
}
