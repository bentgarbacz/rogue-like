using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    public GameObject container;
    private DungeonManager dum;

    void Start()
    {
        
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }

    public void Drop()
    {

        List<Item> droppedItems = LootTableManager.CreateItems(GetComponent<Character>().dropTable);        

        if(droppedItems.Count > 0)
        {

            GameObject lootContainer = Instantiate(container, transform.position, transform.rotation);
            Loot loot = lootContainer.GetComponent<Loot>();

            loot.coord = new Vector2Int((int)transform.position.x, (int)transform.position.z);       
            loot.AddItems(droppedItems);  

            dum.AddGameObject(lootContainer);
            dum.itemContainers.Add(loot);
        }
    }
}
