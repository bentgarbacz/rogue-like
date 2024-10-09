using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    public Item item = null;
    public Button slot;
    public List<Item> itemList;
    public int slotIndex;
    public ItemSlotType type;
    public Sprite defaultSprite;
   
    void Awake()
    {       

        slot.image.sprite = defaultSprite;
    }

    public void AddItem(Item newItem)
    {

        item = newItem;
        slot.image.sprite = item.sprite;
    }

    public void ThrowAway()
    {

        item = null;
        slot.image.sprite = defaultSprite;
        itemList = null;
    }

    public void TransferItem(ItemSlot destinationItemSlot)
    {

        Item itemHold = destinationItemSlot.item;

        if(item != null)
        {

            destinationItemSlot.AddItem(item);
        }

        if(itemList != null)
        {
            
            itemList.RemoveAt(slotIndex);
        }

        if(itemHold == null)
        {

            ThrowAway();

        }else
        {

            AddItem(itemHold);
        }
    }

    public string GetSummary()
    {

        string summary = item.title;

        if(item is Equipment equipment)
        {

            foreach(StatType bonus in equipment.bonusStatDictionary.Keys)
            {

                if(equipment.bonusStatDictionary[bonus] != 0)
                {

                    summary += "\n" + bonus.ToString() + ": " + equipment.bonusStatDictionary[bonus].ToString();
                }
            }
        }        

        return summary + "\n" + item.description;        
    }
}
