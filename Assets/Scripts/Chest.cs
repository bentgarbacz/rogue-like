using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Loot
{
    public Mesh openMesh;
    public string lootTable = "Chest";
    
    void Start()
    {   
        GameObject managers = GameObject.Find("System Managers");

        uiam = managers.GetComponent<UIActiveManager>();
        dum = managers.GetComponent<DungeonManager>();

        audioSource = GetComponent<AudioSource>();

        
        AddItems(LootTableReferences.CreateItems(lootTable));  
        dum.itemContainers.Add(this);
    }

    public override void DiscardIfEmpty()
    {

        if(ItemCount() == 0)
        {

            GetComponent<MeshFilter>().mesh = openMesh;
            enabled = false; 
        }
    }

    public override void OpenContainer()
    {

        base.OpenContainer();
        DiscardIfEmpty();
    }
}
