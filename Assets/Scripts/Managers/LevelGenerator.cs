using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.VisualScripting;
using System.Linq;

[RequireComponent(typeof(CaveBiome))]
[RequireComponent(typeof(CatacombBiome))]
[RequireComponent(typeof(DebugBiome))]

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private EntityManager entityMgr;
    [SerializeField] private TileManager tileMgr;
    [SerializeField] private MiniMapManager miniMapManager;
    [SerializeField] private LockManager lockMgr;
    [SerializeField] private GameObject wallJoiner;
    [SerializeField] private List<GameObject> debugObjects = new();
    public Dictionary<BiomeType, Biome> biomeDict;
    private bool generatingLevel = false;
    
    void Start()
    {

        biomeDict = new Dictionary<BiomeType, Biome>
        {
            
            { BiomeType.Catacomb, GetComponent<CatacombBiome>() },
            { BiomeType.Cave, GetComponent<CaveBiome>() },
            { BiomeType.Debug, GetComponent<DebugBiome>() }
        };
        
        NewLevel(biomeDict[BiomeType.Cave]);        
    }

    public void NewLevel(Biome biome)
    {
        if(generatingLevel)
        {
            
            return;
        }

        generatingLevel = true;

        Vector2Int firstTileCoord;

        entityMgr.CleanUp();
        firstTileCoord = biome.GenerateLevel(tileMgr.levelCoords);    
        miniMapManager.DrawIcons(entityMgr.entitiesInLevel);
        miniMapManager.UpdateDynamicIcons();
        entityMgr.AddGameObject(entityMgr.hero);
        
        entityMgr.hero.SetActive(false);

        foreach (GameObject dbg in debugObjects)
        {
            if (dbg != null)
                dbg.SetActive(false);
        }

        StartCoroutine(DelayedPostGeneration(firstTileCoord));
    }

    private void MoveDebugObjects()
    {

        foreach(GameObject debugObject in debugObjects)
        {
            Chest chest = debugObject.GetComponent<Chest>();
            Exit exit = debugObject.GetComponent<Exit>();

            if(chest != null)
            {

                debugObject.transform.position = new Vector3(entityMgr.hero.transform.position.x - 3, debugObject.transform.position.y, entityMgr.hero.transform.position.z);
                chest.loc.coord = entityMgr.playerCharacter.loc.coord;

            }else if(exit != null)
            {

                debugObject.transform.position = new Vector3(entityMgr.hero.transform.position.x + 3, debugObject.transform.position.y, entityMgr.hero.transform.position.z);
                debugObject.GetComponent<ObjectVisibility>().Initialize(true);
                exit.loc.coord = entityMgr.playerCharacter.loc.coord;
            }
        }
    }

    private IEnumerator DelayedPostGeneration(Vector2Int firstTileCoord)
    {
        yield return new WaitForSeconds(0.5f);

        entityMgr.playerCharacter.Teleport(firstTileCoord);

        entityMgr.hero.SetActive(true);

        foreach (GameObject dbg in debugObjects)
        {
            if (dbg != null)
                dbg.SetActive(true);
        }

        MoveDebugObjects();
        generatingLevel = false;
    }

    public bool IsGenerating()
    {
        
        return generatingLevel;
    }
}
