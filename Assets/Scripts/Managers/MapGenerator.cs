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
    public GameObject floorTile;
    public GameObject floorTile1;
    public GameObject floorTile2;
    public GameObject floorTile3;
    public GameObject floorTile4;
    public GameObject wallTile;
    public GameObject wallJoiner;
    public GameObject enterance;
    public GameObject exit;
    public GameObject chest;
    public GameObject skeleton;
    public GameObject goblin;
    public GameObject rat;
    public GameObject skeletonArcher;
    private DungeonManager dum;
    private Vector2Int startPosition = new(0, 0);
    
    void Start()
    {

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        NewLevel();        
    }

    public void NewLevel()
    {

        dum.dungeonCoords = new HashSet<Vector2Int>();
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

                        GameObject newEnterance = Instantiate(enterance, spawnPos, enterance.transform.rotation);
                        dum.AddGameObject(newEnterance);
                        newEnterance.GetComponent<Tile>().SetCoord(position);

                    //potentially spawn exit after a certain number of steps
                    }else if(i == repeatWalks - 1 && j > walkLength / 2 && exitSpawned == false)
                    {

                        exitSpawned = true;
                        spawnPos.y -= 0.88f;
                        GameObject newExit = Instantiate(exit, spawnPos, exit.transform.rotation);

                        foreach(Vector2Int direction in Direction2D.cardinalDirectionsList)
                        {

                            if(path.Contains(position + direction))
                            {
                                
                                newExit.transform.rotation = Quaternion.Euler(0, Rules.DetermineRotation(newExit.transform.position, new Vector3(position.x + direction.x, 0, position.y + direction.y)), 0);
                                break;
                            }
                        }

                        dum.AddGameObject(newExit);
                        newExit.GetComponent<Exit>().coord = position;

                    
                    //Otherwise spawn a normal tile
                    }else
                    {

                        GameObject newTile;

                        if(spawnRNG <= 1000 && spawnRNG >= 997)
                        {
                            
                            newTile = Instantiate(floorTile1, spawnPos, floorTile1.transform.rotation);
                            dum.AddGameObject(newTile);

                        }else if(spawnRNG <= 996 && spawnRNG >= 993)
                        {

                            newTile = Instantiate(floorTile2, spawnPos, floorTile2.transform.rotation);
                            dum.AddGameObject(newTile);

                        }else if(spawnRNG <= 992 && spawnRNG >= 989)
                        {

                            newTile = Instantiate(floorTile3, spawnPos, floorTile3.transform.rotation);
                            dum.AddGameObject(newTile);

                        }else if(spawnRNG <= 988 && spawnRNG >= 985)
                        {

                            newTile = Instantiate(floorTile4, spawnPos, floorTile4.transform.rotation);
                            dum.AddGameObject(newTile);

                        }else
                        {

                            newTile = Instantiate(floorTile, spawnPos, floorTile.transform.rotation);
                            dum.AddGameObject(newTile);
                        }
                        
                        newTile.GetComponent<Tile>().SetCoord(position);
                    }

                    spawnPos.y += 0.1f;

                    //move player to first tile generated
                    if(i == 0 && j == 1)
                    {

                        dum.hero.GetComponent<Character>().Move(spawnPos, dum.occupiedlist);                    
                    }

                    if(spawnRNG >= 0 && spawnRNG <= 2)
                    {

                        GameObject c = Instantiate(chest, spawnPos, chest.transform.rotation);
                        dum.AddGameObject(c);
                        c.GetComponent<Loot>().coord = new Vector2Int((int)spawnPos.x, (int)spawnPos.z);

                    }else if(spawnRNG >= 3 && spawnRNG <= 4)
                    {

                        GameObject enemy = Instantiate(skeleton, spawnPos, skeleton.transform.rotation);
                        dum.AddGameObject(enemy);
                        enemy.GetComponent<Character>().Move(spawnPos, dum.occupiedlist);
                        dum.enemies.Add(enemy);

                    }else if(spawnRNG >= 5 && spawnRNG <= 6)
                    {

                        //GameObject enemy = Instantiate(goblin, spawnPos, goblin.transform.rotation);
                        GameObject enemy = Instantiate(skeletonArcher, spawnPos, skeletonArcher.transform.rotation);

                        dum.AddGameObject(enemy);
                        enemy.GetComponent<Character>().Move(spawnPos, dum.occupiedlist);
                        dum.enemies.Add(enemy);

                    }else if(spawnRNG >= 7 && spawnRNG <= 8)
                    {

                        GameObject enemy = Instantiate(rat, spawnPos, rat.transform.rotation);
                        dum.AddGameObject(enemy);
                        enemy.GetComponent<Character>().Move(spawnPos, dum.occupiedlist);
                        dum.enemies.Add(enemy);
                    }
                    
                    
                    path.Add(position);
                }

                position += Direction2D.getRandomDirection();
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

            foreach(Vector2Int direction in Direction2D.intercardinalDirectionsList.Concat(Direction2D.cardinalDirectionsList))            
            {

                Vector2Int checkCoord = coord + direction;

                if(!path.Contains(checkCoord) && !wallMap.Contains(checkCoord))
                {
                    
                    Vector3 spawnPos = new(checkCoord.x, 0, checkCoord.y);
                    GameObject newWall = Instantiate(wallTile, spawnPos, wallTile.transform.rotation);
                    newWall.GetComponent<Tile>().SetCoord(new Vector2Int((int)spawnPos.x, (int)spawnPos.z));
                    dum.AddGameObject(newWall);
                    newWall.GetComponent<Renderer>().material.color = Color.gray;
                    wallMap.Add(checkCoord);

                    foreach(Vector2Int direction2 in Direction2D.intercardinalDirectionsList.Concat(Direction2D.cardinalDirectionsList))
                    {

                        Vector2Int checkCoordWall = checkCoord + direction2;
                        Vector2 checkCoordWallJoiner = checkCoordWall;
                        checkCoordWallJoiner.x -= (float) direction2.x / 2;
                        checkCoordWallJoiner.y -= (float) direction2.y / 2;

                        if(wallMap.Contains(checkCoordWall) && !wallJoinerMap.Contains(checkCoordWallJoiner))
                        {

                            Vector3 joinerSpawnPos = new(checkCoordWallJoiner.x, 0, checkCoordWallJoiner.y);

                            GameObject newWallJoiner = Instantiate(wallJoiner, joinerSpawnPos, wallJoiner.transform.rotation);
                            dum.AddGameObject(newWallJoiner);
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

    public static Vector2Int getRandomDirection()
    {

        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }
}
