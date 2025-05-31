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

        dum.CleanUp();
        biome.GenerateLevel(dum.dungeonCoords);    
        miniMapManager.DrawIcons(dum.dungeonSpecificGameObjects);
        miniMapManager.UpdateDynamicIcons();
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
                chest.coord = dum.playerCharacter.coord;

            }else if(exit != null)
            {

                debugObject.transform.position = new Vector3(dum.hero.transform.position.x + 3, debugObject.transform.position.y, dum.hero.transform.position.z);
                exit.coord = dum.playerCharacter.coord;
            }
        }
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new()
    {

        new Vector2Int(0, 1), //North
        new Vector2Int(1, 0), //East
        new Vector2Int(0, -1), //South
        new Vector2Int(-1, 0) //West
    };

    public static List<Vector2Int> intercardinalDirectionsList = new()
    {

        new Vector2Int(1, 1), //North East
        new Vector2Int(1, -1), //South East
        new Vector2Int(-1, -1), //South West
        new Vector2Int(-1, 1) //North West
    };

    public static List<Vector2Int> DirectionsList()
    {

        return intercardinalDirectionsList.Concat(cardinalDirectionsList).ToList();
    }

    public static Vector2Int GetRandomDirection()
    {

        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }
}

public static class LevelGenFuncs
{

    
}
