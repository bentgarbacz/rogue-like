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
    public string type;
   

    public void AddItem(Item newItem)
    {

        item = newItem;
        slot.image.sprite = item.sprite;
    }

    public void ThrowAway()
    {

        item = null;
        slot.image.sprite = null;
        itemList = null;
    }

    public void TransferItem(ItemSlot destinationItemSlot)
    {

        Item itemHold = destinationItemSlot.item;
        destinationItemSlot.AddItem(item);

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
}
