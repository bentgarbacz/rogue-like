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
    private EntityManager entityMgr;
    private MiniMapManager miniMapManager;
    private VisibilityManager visibilityManager;
    private TileManager tileMgr;


    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");

        entityMgr = managers.GetComponent<EntityManager>();
        miniMapManager = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
        visibilityManager = managers.GetComponent<VisibilityManager>();
        tileMgr = managers.GetComponent<TileManager>();
    }

    public void Drop()
    {

        List<Item> droppedItems = LootTableReferences.CreateItems(GetComponent<CharacterSheet>().dropTable);

        if (droppedItems.Count > 0)
        {

            GameObject dropContainer = GameFunctions.DropLoot(this.gameObject, container, droppedItems, entityMgr, tileMgr, miniMapManager, visibilityManager);
            Vector2Int containerCoord = dropContainer.GetComponent<ObjectLocation>().coord;
            tileMgr.tileDict[containerCoord].AddEntity(dropContainer);
        }
    }
}
