using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DropLoot : MonoBehaviour
{
    public GameObject container;
    private DungeonManager dum;
    private MiniMapManager miniMapManager;


    void Start()
    {
        
        GameObject managers = GameObject.Find("System Managers");
        
        dum = managers.GetComponent<DungeonManager>();
        miniMapManager = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
    }

    public void Drop()
    {

        List<Item> droppedItems = LootTableReferences.CreateItems(GetComponent<CharacterSheet>().dropTable);        

        if(droppedItems.Count > 0)
        {

            GameFunctions.DropLoot(this.gameObject, container, droppedItems, dum, miniMapManager);
        }
    }
}
