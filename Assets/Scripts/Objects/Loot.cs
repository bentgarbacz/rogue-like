using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : Interactable
{

    public List<Item> items;
    protected UIActiveManager uiam;
    protected DungeonManager dum;
    protected AudioSource audioSource;

    void Start()
    {
        
        GameObject managers = GameObject.Find("System Managers");

        uiam = managers.GetComponent<UIActiveManager>();
        dum = managers.GetComponent<DungeonManager>();

        audioSource = GetComponent<AudioSource>();
    }

    public void AddItems(List<Item> newItems)
    {

        items = newItems;
    }

    public virtual void OpenContainer()
    {

        audioSource.Play();

        uiam.OpenLootPanel(this);        
        uiam.OpenInventoryPanel();
    }

    public override void Interact()
    {

        OpenContainer();
    }

    public int ItemCount()
    {

        return items.Count;
    }

    public void Discard()
    {

        dum.TossContainer(gameObject);
    }

    public virtual void DiscardIfEmpty()
    {

        if(ItemCount() == 0)
        {
        
            Discard();
        }
    }
}
