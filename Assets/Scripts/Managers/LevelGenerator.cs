using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.VisualScripting;
using System.Linq;


public class LevelGenerator : MonoBehaviour
{

    private DungeonManager dum;
    [SerializeField] private MiniMapManager miniMapManager;
    [SerializeField] private GameObject wallJoiner;
    [SerializeField] private List<GameObject> debugObjects = new();
    private NPCGenerator npcGen;
    private Vector2Int startPosition = new(0, 0);
    public Dictionary<BiomeType, Biome> biomeDict;
    
    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        npcGen = GetComponent<NPCGenerator>();

        biomeDict = new Dictionary<BiomeType, Biome>
        {
            
            { BiomeType.Catacomb, GetComponent<CatacombBiome>() },
            { BiomeType.Cave, GetComponent<CaveBiome>() }
        };
        
        NewLevel(biomeDict[BiomeType.Cave]);        
    }

    public void NewLevel(Biome biome)
    {

        dum.dungeonCoords = new();
        biome.GenerateLevel(startPosition, dum.dungeonCoords, dum, npcGen);    
        GenerateWalls(dum.dungeonCoords, biome);
        miniMapManager.DrawIcons(dum.dungeonSpecificGameObjects);
        miniMapManager.UpdateDynamicIcons();
        MoveDebugObjects();
    }

    private void GenerateWalls(HashSet<Vector2Int> path, Biome biome)
    {

        //finds border tiles and places walls adjacent to them
        HashSet<Vector2Int> wallMap = new();
        HashSet<Vector2> wallJoinerMap = new();

        foreach(Vector2Int coord in path)
        {

            foreach(Vector2Int direction1 in Direction2D.DirectionsList())            
            {

                Vector2Int checkCoord = coord + direction1;

                if(!path.Contains(checkCoord) && !wallMap.Contains(checkCoord))
                {
                    
                    biome.CreateWallTile(new Vector3(checkCoord.x, 0, checkCoord.y), dum);
                    wallMap.Add(checkCoord);

                    foreach(Vector2Int direction2 in Direction2D.DirectionsList())
                    {

                        Vector2Int checkCoordWall = checkCoord + direction2;
                        Vector2 checkCoordWallJoiner = checkCoordWall;
                        checkCoordWallJoiner.x -= (float) direction2.x / 2;
                        checkCoordWallJoiner.y -= (float) direction2.y / 2;

                        if(wallMap.Contains(checkCoordWall) && !wallJoinerMap.Contains(checkCoordWallJoiner))
                        {

                            CreateWallJoiner(new Vector3(checkCoordWallJoiner.x, 0, checkCoordWallJoiner.y), dum);
                            wallJoinerMap.Add(checkCoordWallJoiner);
                        }
                    }
                }
            }
        }
    }

    public void CreateWallJoiner(Vector3 spawnPos, DungeonManager dum)
    {

        GameObject newWallJoiner = Instantiate(wallJoiner, spawnPos, wallJoiner.transform.rotation);
        dum.AddGameObject(newWallJoiner);
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
