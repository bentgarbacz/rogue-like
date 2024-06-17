using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<Item> dropTable = new List<Item>();
    public Mesh openMesh;
    private Loot loot;
    private DungeonManager dum;
    
    void Start()
    {   

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        loot = GetComponent<Loot>();

        loot.AddItems(LootTableManager.CreateItems("Chest"));  
        dum.itemContainers.Add(loot);
    }

    void Update()
    {
        
        if(loot.ItemCount() == 0)
        {

            GetComponent<MeshFilter>().mesh = openMesh;
            enabled = false; 
        }
    }
}
