using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    public List<ItemSlot> lootSlots = new List<ItemSlot>();
    public Dictionary<string, ItemSlot> equipmentSlotsDictionary = new Dictionary<string, ItemSlot>();
    private readonly int inventorySlotCount = 24;
    private readonly int lootSlotCount = 8;
    private readonly int equipmentSlotCount = 9;
    private DungeonManager dum;
    private UIActiveManager uiam;
    private EquipmentManager equm;

    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        equm = GameObject.Find("System Managers").GetComponent<EquipmentManager>();

        GameObject invGrid = uiam.inventoryPanel.transform.GetChild(0).gameObject;
        GameObject lootGrid = uiam.lootPanel.transform.GetChild(0).gameObject;
        GameObject equipmentGrid = uiam.equipmentPanel.transform.GetChild(0).gameObject;

        ConstructItemSlotList(inventorySlots, invGrid, inventorySlotCount);
        ConstructItemSlotList(lootSlots, lootGrid, lootSlotCount);

        for(int i = 0; i < equipmentSlotCount; i++)
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

                //transfer item to inventory and handle UI logic
                List<Item> holdItemList = targetSlot.itemList;

                targetSlot.TransferItem(i);

                targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();    

                //if taking loot, handle loot panel logic
                if(targetSlot.type == "Loot")
                {  
                             
                    //uiam.CloseLootPanel();
                    uiam.OpenLootPanel(holdItemList);

                }else if(equipmentSlotsDictionary[targetSlot.type])
                {

                    equm.UpdateStats(equipmentSlotsDictionary);
                }

                targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();

                break; 
            }
        }
    }

    public ItemSlot GetEquipmentSlot(Equipment equipItem)
    {


        if(equipItem.equipmentType == "Ring")
        {

            if(equipmentSlotsDictionary["Ring 1"].item != null && equipmentSlotsDictionary["Ring 2"].item == null)
            {

                return equipmentSlotsDictionary["Ring 2"];

            }else
            {
                
                return equipmentSlotsDictionary["Ring 1"];
            }
        
        }else if(equipItem.equipmentType == "Switch Hand")
        {
        
            if(equipmentSlotsDictionary["Main Hand"].item != null && equipmentSlotsDictionary["Off Hand"].item == null)
            {

                return equipmentSlotsDictionary["Off Hand"];

            }else
            {
                
                return equipmentSlotsDictionary["Main Hand"];
            }

        }else
        {

            return equipmentSlotsDictionary[equipItem.equipmentType];
        }
    }

    public void UpdateStats()
    {

        equm.UpdateStats(equipmentSlotsDictionary);
    }

    public void ContextualAction(ItemSlot targetSlot)
    {

        //define behavior for click on inventory contextual button
        if(targetSlot.item is Consumable consumable)
        {
            
            consumable.Use();
            targetSlot.ThrowAway();
            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();
            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();

        }else if(targetSlot.item is Equipment equipment)
        {

            targetSlot.TransferItem(GetEquipmentSlot(equipment));
            UpdateStats();
            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseExit();
            targetSlot.slot.GetComponent<MouseOverItemSlot>().MouseEnter();
        }
    }
}
