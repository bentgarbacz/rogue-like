using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEmptyLoot : MonoBehaviour
{

    private Loot loot;
    private DungeonManager dum;

    void Start()
    {
        
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        loot = GetComponent<Loot>();
    }

    void Update()
    {
        
        if(loot.ItemCount() == 0)
        {

            dum.TossContainer(gameObject);
        }
    }
}
