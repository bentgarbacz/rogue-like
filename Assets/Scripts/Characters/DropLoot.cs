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
        
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        miniMapManager = GameObject.Find("CanvasHUD").transform.Find("Map Panel").GetComponentInChildren<MiniMapManager>();
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
