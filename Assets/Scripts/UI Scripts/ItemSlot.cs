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
   
    void Awake()
    {
        

        SetVisible(false);
    }

    public void AddItem(Item newItem)
    {

        item = newItem;
        slot.image.sprite = item.sprite;
        SetVisible(true);
    }

    public void ThrowAway()
    {

        item = null;
        slot.image.sprite = null;
        SetVisible(false);
        itemList = null;
    }

    public void SetVisible(bool isVisable)
    {

        Color color = slot.image.color;

        if(isVisable)
        {

            color.a = 1f;

        }else{

            color.a = 0f;
        }

        slot.image.color = color;
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
