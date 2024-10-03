using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

        List<Item> droppedItems = LootTableReferences.CreateItems(GetComponent<CharacterSheet>().dropTable);        

        if(droppedItems.Count > 0)
        {

            //Determine drop location and introduce randomness to make multiple loot instances clickable on a single tile
            Vector3 dropPos = transform.position;
            dropPos.x += (float)(Random.Range(-20, 20) * 0.01);
            dropPos.z += (float)(Random.Range(-20, 20) * 0.01);

            GameObject lootContainer = Instantiate(container, dropPos, transform.rotation);
            Loot loot = lootContainer.GetComponent<Loot>();

            //Sets the tile coordinate in which the loot resides
            loot.coord = new Vector2Int((int)transform.position.x, (int)transform.position.z);       
            loot.AddItems(droppedItems);  

            dum.AddGameObject(lootContainer);
            dum.itemContainers.Add(loot);
        }
    }
}
