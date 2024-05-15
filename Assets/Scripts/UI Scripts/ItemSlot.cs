using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    public Item item;
    public Button slot;
    void Start()
    {

        item = null;
    }

    public void AddItem(Item newItem)
    {

        item = newItem;
        slot.image.sprite = item.sprite;
    }

    public void ThrowAway()
    {

        item = null;
        slot.image.sprite = null;
    }

    public void TransferItem(ItemSlot transferItemSlot)
    {

        Item itemHold = transferItemSlot.item;
        transferItemSlot.AddItem(item);

        if(transferItemSlot.item == null)
        {
            ThrowAway();

        }else
        {

            AddItem(itemHold);
        }
    }
}
