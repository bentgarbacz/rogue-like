using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<Item> dropTable = new List<Item>();
    private DungeonManager dum;
    
    void Start()
    {   

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();

        Loot loot = GetComponent<Loot>();

        loot.AddItems(LootTableManager.CreateItems("Chest"));  
        dum.itemContainers.Add(loot);
    }
}
