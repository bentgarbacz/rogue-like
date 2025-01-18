using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.VisualScripting;
using System.Linq;


public class MapGenerator : MonoBehaviour
{

    public int walkLength;
    private DungeonManager dum;
    private NPCGenerator npcGen;
    private TileReferences tileRef;
    private Vector2Int startPosition = new(0, 0);
    
    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        npcGen = GetComponent<NPCGenerator>();
        tileRef = GetComponent<TileReferences>();
        NewLevel();        
    }

    public void NewLevel()
    {

        dum.dungeonCoords = new();
        LevelGen(startPosition, walkLength, 100, dum.dungeonCoords);    
        GenerateWalls(dum.dungeonCoords);
        //dum.cachedPathsDict = PrecacheMapPaths(path);
    }

    private void LevelGen(Vector2Int position, int walkLength, int repeatWalks, HashSet<Vector2Int> path)
    {

        bool exitSpawned = false;

        for(int i = 0; i < repeatWalks; i++)
        {

            for(int j = 0; j < walkLength; j++)
            {
                
                if( !path.Contains(position) )
                {

                    Vector3 spawnPos = new(position.x, 0, position.y);
                    int spawnRNG = UnityEngine.Random.Range(0, 1000);

                    //spawn enterance on first tile
                    if(i == 0 && j == 0)
                    {

                        tileRef.CreateEntranceTile(BiomeType.Catacomb, spawnPos, position, dum);

                    //potentially spawn exit after a certain number of steps
                    }else if(i == repeatWalks - 1 && j > walkLength / 2 && exitSpawned == false)
                    {

                        tileRef.CreateExitTile(BiomeType.Catacomb, spawnPos, position, path, dum);
                        exitSpawned = true;
                    
                    //Otherwise spawn a normal tile
                    }else
                    {

                        tileRef.CreateTile(BiomeType.Catacomb, spawnPos, position, spawnRNG, dum);                        
                    }

                    //move player to first tile generated
                    if(i == 0 && j == 1)
                    {

                        dum.hero.GetComponent<CharacterSheet>().Move(spawnPos, dum.occupiedlist);                    
                    }

                    if(spawnRNG >= 0 && spawnRNG <= 2)
                    {

                        npcGen.CreateChest(spawnPos, dum);

                    }else if(spawnRNG >= 3 && spawnRNG <= 4)
                    {

                        npcGen.CreateNPC(NPCType.Slime, spawnPos, dum);

                    }else if(spawnRNG >= 5 && spawnRNG <= 6)
                    {

                        npcGen.CreateNPC(NPCType.Witch, spawnPos, dum);

                    }else if(spawnRNG >= 7 && spawnRNG <= 8)
                    {

                        npcGen.CreateNPC(NPCType.GoatMan, spawnPos, dum);
                    }
                    
                    path.Add(position);
                }

                position += Direction2D.GetRandomDirection();
            }
        }
    }

    public Dictionary<string, List<Vector2Int>> PrecacheMapPaths(HashSet<Vector2Int> p)
    {

        Dictionary<string, List<Vector2Int>> pathsDictionary = new();

        foreach(Vector2Int node1 in dum.dungeonCoords)
        {   

            foreach(Vector2Int node2 in dum.dungeonCoords)
            {
                                
                if(node1 != node2)
                {

                    double distance = Math.Sqrt(Math.Pow((node2.x - node1.x), 2) + Math.Pow((node2.y - node1.y), 2));

                    if(distance > 3){

                        continue;
                    }

                    PathFinder.PrecachePath(node1, node2, dum.dungeonCoords, pathsDictionary);
                }
            }
        }

        return pathsDictionary;
    }

    private void GenerateWalls(HashSet<Vector2Int> path){

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
                    
                    tileRef.CreateWallTile(BiomeType.Catacomb, new Vector3(checkCoord.x, 0, checkCoord.y), dum);
                    wallMap.Add(checkCoord);

                    foreach(Vector2Int direction2 in Direction2D.DirectionsList())
                    {

                        Vector2Int checkCoordWall = checkCoord + direction2;
                        Vector2 checkCoordWallJoiner = checkCoordWall;
                        checkCoordWallJoiner.x -= (float) direction2.x / 2;
                        checkCoordWallJoiner.y -= (float) direction2.y / 2;

                        if(wallMap.Contains(checkCoordWall) && !wallJoinerMap.Contains(checkCoordWallJoiner))
                        {

                            tileRef.CreateWallJoiner(new Vector3(checkCoordWallJoiner.x, 0, checkCoordWallJoiner.y), dum);
                            wallJoinerMap.Add(checkCoordWallJoiner);
                        }
                    }
                }
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
