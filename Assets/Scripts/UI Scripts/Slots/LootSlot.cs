using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSlot : ItemSlot
{

    public Loot parentLoot;

    public override void TransferItem(ItemSlot destinationItemSlot)
    {

        Item itemHold = destinationItemSlot.item;
        idm.ForgetItem();

        if(item != null)
        {

            destinationItemSlot.AddItem(item);

            parentLoot.items.RemoveAt(slotIndex);
            parentLoot.DiscardIfEmpty();
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
