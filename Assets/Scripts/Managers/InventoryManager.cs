using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> lootSlots = new List<ItemSlot>();
    private readonly int inventorySlotCount = 24;
    public GameObject inventoryPanel;
    public bool inventoryIsOpen = true;
    public GameObject lootPanel;
    private readonly int lootSlotCount = 8;
    public bool lootIsOpen = true;

    void Start()
    {

        InactiveReferences ir = GameObject.Find("CanvasHUD").GetComponent<InactiveReferences>();
        inventoryPanel = ir.InventoryPanel;
        lootPanel = ir.LootPanel;        

        GameObject invGrid = inventoryPanel.transform.GetChild(0).gameObject;
        GameObject lootGrid = lootPanel.transform.GetChild(0).gameObject;

        CloseInventoryPanel();
        CloseLootPanel();

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

    public bool IsPointerOverUI()
    {

        return EventSystem.current.IsPointerOverGameObject();
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

        if(inventoryIsOpen == false)
        {

            inventoryIsOpen = true;
            inventoryPanel.SetActive(inventoryIsOpen);
        }
    }

    public void CloseInventoryPanel()
    {

        
        if(inventoryIsOpen == true)
        {

            inventoryIsOpen = false;
            inventoryPanel.SetActive(inventoryIsOpen);
        }
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

    public void OpenLootPanel(List<Item> items)
    {

        foreach(Item i in items)
        {

            foreach(ItemSlot slot in lootSlots)
            {

                if(slot.item == null)
                {

                    slot.AddItem(i);
                    slot.itemList = items;
                    break;
                }
            }
        }
    
        if(lootIsOpen == false)
        {

            lootIsOpen = true;
            lootPanel.SetActive(lootIsOpen);
        }
    }

    public void CloseLootPanel()
    {

        if(lootIsOpen == true)
        {

            lootIsOpen = false;
            lootPanel.SetActive(lootIsOpen);

            ClearItemsLoot();
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
               CloseLootPanel();
               OpenLootPanel(holdItemList);
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
