using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<Item> dropTable = new();
    public Mesh openMesh;
    public string lootTable = "Chest";
    private Loot loot;
    private DungeonManager dum;
    
    void Start()
    {   

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        loot = GetComponent<Loot>();

        loot.AddItems(LootTableReferences.CreateItems(lootTable));  
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
