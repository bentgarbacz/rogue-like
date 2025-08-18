using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.VisualScripting;
using System.Linq;


public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private DungeonManager dum;
    [SerializeField] private MiniMapManager miniMapManager;
    [SerializeField] private GameObject wallJoiner;
    [SerializeField] private List<GameObject> debugObjects = new();
    public Dictionary<BiomeType, Biome> biomeDict;
    
    void Start()
    {

        biomeDict = new Dictionary<BiomeType, Biome>
        {
            
            { BiomeType.Catacomb, GetComponent<CatacombBiome>() },
            { BiomeType.Cave, GetComponent<CaveBiome>() }
        };
        
        NewLevel(biomeDict[BiomeType.Cave]);        
    }

    public void NewLevel(Biome biome)
    {

        Vector2Int firstTileCoord;

        dum.CleanUp();
        firstTileCoord = biome.GenerateLevel(dum.dungeonCoords);    
        miniMapManager.DrawIcons(dum.dungeonSpecificGameObjects);
        miniMapManager.UpdateDynamicIcons();
        dum.hero.GetComponent<CharacterSheet>().Move(firstTileCoord); 
        MoveDebugObjects();
    }

    private void MoveDebugObjects()
    {

        foreach(GameObject debugObject in debugObjects)
        {
            Chest chest = debugObject.GetComponent<Chest>();
            Exit exit = debugObject.GetComponent<Exit>();

            if(chest != null)
            {

                debugObject.transform.position = new Vector3(dum.hero.transform.position.x - 3, debugObject.transform.position.y, dum.hero.transform.position.z);
                chest.loc.coord = dum.playerCharacter.loc.coord;

            }else if(exit != null)
            {

                debugObject.transform.position = new Vector3(dum.hero.transform.position.x + 3, debugObject.transform.position.y, dum.hero.transform.position.z);
                debugObject.GetComponent<ObjectVisibility>().Initialize(true);
                exit.loc.coord = dum.playerCharacter.loc.coord;
            }
        }
    }
}
