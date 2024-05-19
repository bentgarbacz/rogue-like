using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    public Item item = null;
    public Button slot;
    private DungeonManager dum;
    

    public void AddItem(Item newItem)
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        item = newItem;
        slot.image.sprite = item.sprite;
    }

    public void ThrowAway()
    {

        item = null;
        slot.image.sprite = null;
    }

    public void TransferItem(ItemSlot destinationItemSlot)
    {

        Item itemHold = destinationItemSlot.item;
        destinationItemSlot.AddItem(item);

        dum.RemoveItem(item.itemID);

        if(itemHold == null)
        {
            
            ThrowAway();

        }else
        {

            AddItem(itemHold);
        }
    }
}
