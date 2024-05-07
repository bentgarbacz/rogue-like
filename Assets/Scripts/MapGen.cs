using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;


public class MapGen : MonoBehaviour
{


    public int walkLength;
    Vector2Int startPosition = new Vector2Int(0, 0);
    public GameObject floorTile;
    public GameObject floorTile1;
    public GameObject floorTile2;
    public GameObject floorTile3;
    public GameObject floorTile4;
    public GameObject wallTile;
    public GameObject enterance;
    public GameObject exit;
    public GameObject hero;
    public GameObject chest;
    public GameObject skeleton;
    public GameObject goblin;
    public GameObject mainCamera;
    public HashSet<Vector2Int> path;
    public HashSet<GameObject> enemies = new HashSet<GameObject>();
    public HashSet<GameObject> LevelSpecificGameObjects = new HashSet<GameObject>();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new Dictionary<string, List<Vector2Int>>();
    public HashSet<Vector3> occupiedlist = new HashSet<Vector3>();
    
    void Start()
    {

        NewLevel();        
    }

    public void NewLevel()
    {

        path = new HashSet<Vector2Int>();
        roomGen(startPosition, walkLength, 100, path);    
        wallGen(path);
        //cachedPathsDict = PrecacheMapPaths(path);
    }

    private void roomGen(Vector2Int position, int walkLength, int repeatWalks, HashSet<Vector2Int> path)
    {

        bool exitSpawned = false;

        for(int i = 0; i < repeatWalks; i++)
        {

            for(int j = 0; j < walkLength; j++)
            {
                
                if( !path.Contains(position) )
                {

                    Vector3 spawnPos = new Vector3(position.x, 0, position.y);
                    int spawnRNG = UnityEngine.Random.Range(0, 1000);

                    //spawn enterance on first tile
                    if(i == 0 && j == 0)
                    {

                        var newTile = Instantiate(enterance, spawnPos, enterance.transform.rotation);
                        LevelSpecificGameObjects.Add(newTile);
                        newTile.GetComponent<Tile>().setCoord(position);

                    //potentially spawn exit after a certain number of steps
                    }else if(i == repeatWalks - 1 && j > walkLength / 2 && exitSpawned == false)
                    {

                        exitSpawned = true;
                        spawnPos.y -= 0.85f;
                        var newTile = Instantiate(exit, spawnPos, exit.transform.rotation);
                        LevelSpecificGameObjects.Add(newTile);
                        newTile.GetComponent<Tile>().setCoord(position);

                    
                    //Otherwise spawn a normal tile
                    }else
                    {

                        GameObject newTile;

                        if(spawnRNG <= 1000 && spawnRNG >= 997)
                        {
                            
                            newTile = Instantiate(floorTile1, spawnPos, floorTile.transform.rotation);
                            LevelSpecificGameObjects.Add(newTile);

                        }else if(spawnRNG <= 996 && spawnRNG >= 993)
                        {

                            newTile = Instantiate(floorTile2, spawnPos, floorTile.transform.rotation);
                            LevelSpecificGameObjects.Add(newTile);

                        }else if(spawnRNG <= 992 && spawnRNG >= 989)
                        {

                            newTile = Instantiate(floorTile3, spawnPos, floorTile.transform.rotation);
                            LevelSpecificGameObjects.Add(newTile);

                        }else if(spawnRNG <= 988 && spawnRNG >= 985)
                        {

                            newTile = Instantiate(floorTile4, spawnPos, floorTile.transform.rotation);
                            LevelSpecificGameObjects.Add(newTile);

                        }else
                        {

                            newTile = Instantiate(floorTile, spawnPos, floorTile.transform.rotation);
                            LevelSpecificGameObjects.Add(newTile);
                        }
                        
                        newTile.GetComponent<Tile>().setCoord(position);
                    }

                    spawnPos.y += 0.1f;

                    //move player to first tile generated
                    if(i == 0 && j == 1)
                    {

                        hero.GetComponent<Character>().Move(spawnPos, occupiedlist);                    
                        mainCamera.GetComponent<PlayerCamera>().setFocalPoint(hero);
                    }

                    if(spawnRNG >= 0 && spawnRNG <= 2)
                    {

                        GameObject c = Instantiate(chest, spawnPos, chest.transform.rotation);
                        LevelSpecificGameObjects.Add(c);
                        c.GetComponent<Loot>().coord = new Vector2Int((int)spawnPos.x, (int)spawnPos.z);

                    }else if(spawnRNG >= 3 && spawnRNG <= 5)
                    {

                        GameObject e = Instantiate(skeleton, spawnPos, skeleton.transform.rotation);
                        LevelSpecificGameObjects.Add(e);
                        e.GetComponent<Character>().Move(spawnPos, occupiedlist);
                        enemies.Add(e);

                    }else if(spawnRNG >= 6 && spawnRNG <= 8)
                    {

                        GameObject e = Instantiate(goblin, spawnPos, goblin.transform.rotation);
                        LevelSpecificGameObjects.Add(e);
                        e.GetComponent<Character>().Move(spawnPos, occupiedlist);
                        enemies.Add(e);
                    }
                    
                    
                    path.Add(position);
                }

                position += Direction2D.getRandomDirection();
            }
        }
    }

    public Dictionary<string, List<Vector2Int>> PrecacheMapPaths(HashSet<Vector2Int> p)
    {

        Dictionary<string, List<Vector2Int>> pathsDictionary = new Dictionary<string, List<Vector2Int>>();

        foreach(Vector2Int node1 in path)
        {   

            foreach(Vector2Int node2 in path)
            {
                                
                if(node1 != node2)
                {

                    double distance = Math.Sqrt(Math.Pow((node2.x - node1.x), 2) + Math.Pow((node2.y - node1.y), 2));

                    if(distance > 3){

                        continue;
                    }

                    PathFinder.PrecachePath(node1, node2, path, pathsDictionary);
                }
            }
        }

        return pathsDictionary;
    }

    public void CleanUp()
    {
        
        //path is reset in in NewRoom, may need to refactor
        //path = new HashSet<Vector2Int>();
        cachedPathsDict = new Dictionary<string, List<UnityEngine.Vector2Int>>();
        enemies = new HashSet<GameObject>();
        occupiedlist = new HashSet<Vector3>();

        foreach(GameObject trash in LevelSpecificGameObjects)
        {

            Destroy(trash);
        }

        LevelSpecificGameObjects = new HashSet<GameObject>();;
    }

    private void wallGen(HashSet<Vector2Int> path){

        HashSet<Vector2Int> wallMap = new HashSet<Vector2Int>();

        foreach(Vector2Int t in path)
        {
            
            foreach(Vector2Int direction in Direction2D.cardinalDirectionsList)
            {

                Vector2Int checkCoord = t + direction;

                if(!path.Contains(checkCoord) && !wallMap.Contains(checkCoord))
                {
                    
                    Vector3 spawnPos = new Vector3(checkCoord.x, 0, checkCoord.y);
                    var newWall = Instantiate(wallTile, spawnPos, wallTile.transform.rotation);
                    LevelSpecificGameObjects.Add(newWall);
                    newWall.GetComponent<Renderer>().material.color = Color.gray;
                    wallMap.Add(checkCoord);
                }
            }
        }
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {

        new Vector2Int(0, 1), //up
        new Vector2Int(1, 0), //right
        new Vector2Int(0, -1), //down
        new Vector2Int(-1, 0) //left
    };

    public static Vector2Int getRandomDirection()
    {

        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }
}
