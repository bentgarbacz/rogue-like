using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSlot> inventorySlots = new();
    public List<ItemSlot> lootSlots = new();
    public Dictionary<ItemSlotType, ItemSlot> equipmentSlotsDictionary = new();
    private UIActiveManager uiam;
    private EquipmentManager equm;
    private SpellCaster sc;
    private readonly int inventorySlotCount = 24;
    private readonly int lootSlotCount = 8;
    private readonly int equipmentSlotCount = 9;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        uiam = managers.GetComponent<UIActiveManager>();
        equm = managers.GetComponent<EquipmentManager>();
        sc = equm.hero.GetComponent<SpellCaster>();

        GameObject invGrid = uiam.inventoryPanel.transform.GetChild(0).gameObject;
        GameObject lootGrid = uiam.lootPanel.transform.GetChild(0).gameObject;
        GameObject equipmentGrid = uiam.equipmentPanel.transform.GetChild(0).gameObject;

        ConstructItemSlotList(inventorySlots, invGrid, inventorySlotCount); // there are 24 inventory slots
        ConstructItemSlotList(lootSlots, lootGrid, lootSlotCount); // there are 8 loot slots

        for(int i = 0; i < equipmentSlotCount; i++) // there are 9 equipment slots
        {
            ItemSlot slot = equipmentGrid.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<ItemSlot>();

            slot.slotIndex = i;
            equipmentSlotsDictionary.Add(slot.type, slot);
        }
    }

    private void ConstructItemSlotList(List<ItemSlot> slotList, GameObject grid, int children)
    {

        //construct list of item slots that exist in the UI
        for(int i = 0; i < children; i++)
        {

            ItemSlot slot = grid.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<ItemSlot>();

            slot.slotIndex = i; 
            slotList.Add(slot);
        }
    }    

    public void ClearItemsLoot()
    {
        
        //Clear items held in loot slots
        foreach(ItemSlot slot in lootSlots)
        {

            slot.ThrowAway();
        }
    }

    //Puts list of items into loot slots
    public void PopulateLootSlots(List<Item> items)
    {

        ClearItemsLoot();
        
        foreach(Item item in items)
        {

            foreach(ItemSlot slot in lootSlots)
            {
                
                if(slot.item == null)
                {

                    slot.AddItem(item);
                    slot.itemList = items;
                    break;
                }
            }
        }
    }

    public void TakeItem(ItemSlot targetSlot)
    {

        //find empty slot in inventory
        foreach(ItemSlot i in inventorySlots)
        {

            if(i.item == null)
            {

                TransferToInventory(targetSlot, i);
                targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();
                break; 
            }
        }
    }

    public void TransferToInventory(ItemSlot sourceSlot, ItemSlot targetSlot)
    {

        //transfer item to inventory and handle UI logic
        List<Item> holdItemList = sourceSlot.itemList;

        sourceSlot.TransferItem(targetSlot);

        sourceSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();    

        if(sourceSlot.type is not ItemSlotType.Inventory && sourceSlot.type is not ItemSlotType.Drop && sourceSlot.type is not ItemSlotType.Destroy)
        {

            if(sourceSlot.type is ItemSlotType.Loot)
            {  
                            
                uiam.OpenLootPanel(holdItemList);

            }else if(equipmentSlotsDictionary[sourceSlot.type])
            {

                equm.UpdateStats();
            }
        }
    }

    //Given a piece of equipment, return an equipment slot meant for that equipment
    //Will return a deterministic equipment slot if there are multiple eligible slots
    public ItemSlot GetEquipmentSlot(Equipment equipItem)
    {

        if(equipItem.type is EquipmentType.Ring)
        {

            if(equipmentSlotsDictionary[ItemSlotType.RingPrimary].item != null && equipmentSlotsDictionary[ItemSlotType.RingSecondary].item == null)
            {

                return equipmentSlotsDictionary[ItemSlotType.RingSecondary];

            }else
            {
                
                return equipmentSlotsDictionary[ItemSlotType.RingPrimary];
            }
        
        }else if(equipItem.type == EquipmentType.SwitchHand)
        {
        
            if(equipmentSlotsDictionary[ItemSlotType.MainHand].item != null && equipmentSlotsDictionary[ItemSlotType.OffHand].item == null)
            {

                return equipmentSlotsDictionary[ItemSlotType.OffHand];

            }else
            {
                
                return equipmentSlotsDictionary[ItemSlotType.MainHand];
            }

        }else if(equipItem.type is EquipmentType.Helmet)
        {

            return equipmentSlotsDictionary[ItemSlotType.Helmet];

        }else if(equipItem.type is EquipmentType.Amulet)
        {

            return equipmentSlotsDictionary[ItemSlotType.Amulet];

        }else if(equipItem.type is EquipmentType.Chest)
        {

            return equipmentSlotsDictionary[ItemSlotType.Chest];

        }else if(equipItem.type is EquipmentType.Glove)
        {

            return equipmentSlotsDictionary[ItemSlotType.Glove];

        }else if(equipItem.type is EquipmentType.Boot)
        {

            return equipmentSlotsDictionary[ItemSlotType.Boot];

        }else if(equipItem.type is EquipmentType.MainHand)
        {

            return equipmentSlotsDictionary[ItemSlotType.MainHand];

        }else if(equipItem.type is EquipmentType.OffHand)
        {

            return equipmentSlotsDictionary[ItemSlotType.OffHand];
        }

        return null;
    }

    public void UpdateStats()
    {

        equm.UpdateStats();
    }

    public bool ContextualAction(ItemSlot targetSlot)
    {

        bool actionIsSuccessful = false;

        //define behavior for click on inventory contextual button
        if(targetSlot.item is Consumable consumable)
        {
            
            consumable.Use();
            targetSlot.ThrowAway();
            actionIsSuccessful = true;
            RefreshItemSlot(targetSlot);

        }else if(targetSlot.item is Equipment equipment)
        {

            if(equm.MeetsRequirements(equipment))
            {

                targetSlot.TransferItem(GetEquipmentSlot(equipment));
                UpdateStats();
                actionIsSuccessful = true;
                RefreshItemSlot(targetSlot);
            }

        }else if(targetSlot.item is Scroll scroll)
        {

            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();
            uiam.ToggleInventory();
            sc.CastScroll(scroll, targetSlot);
            actionIsSuccessful = true;
        }

        return actionIsSuccessful;
    }

    private void RefreshItemSlot(ItemSlot targetSlot)
    {

        targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();
        targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();
    }
}
