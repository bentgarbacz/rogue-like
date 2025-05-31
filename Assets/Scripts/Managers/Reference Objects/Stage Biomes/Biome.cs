using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{

    [SerializeField] protected DungeonManager dum;
    [SerializeField] protected NPCGenerator npcGen;
    [SerializeField] private GameObject wallJoiner;
    [SerializeField] protected TileManager tileManager;

    public virtual void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG)
    {

        return;
    }

    public virtual void CreateEntranceTile(Vector3 spawnPos, Vector2Int position)
    {

        return;
    }

    public virtual void CreateExitTile(Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> dungeonCoords)
    {

        return;
    }

    public virtual GameObject CreateWallTile(Vector3 spawnPos)
    {

        return null;
    }

    public virtual bool GenerateLevel(HashSet<Vector2Int> dungeonCoords)
    {

        return false;
    }
    
    public virtual void GenerateWalls(HashSet<Vector2Int> dungeonCoords)
    {

        //finds border tiles and places walls adjacent to them
        HashSet<Vector2Int> wallMap = new();
        HashSet<Vector2> wallJoinerMap = new();

        foreach(Vector2Int coord in dungeonCoords)
        {

            foreach(Vector2Int direction1 in Direction2D.DirectionsList())            
            {

                Vector2Int checkCoord = coord + direction1;

                if(!dungeonCoords.Contains(checkCoord) && !wallMap.Contains(checkCoord))
                {
                    
                    GameObject newWallTile = CreateWallTile(new Vector3(checkCoord.x, 0, checkCoord.y));
                    wallMap.Add(checkCoord);

                    foreach(Vector2Int direction2 in Direction2D.cardinalDirectionsList)
                    {

                        Vector2Int checkCoordWall = checkCoord + direction2;
                        Vector2 checkCoordWallJoiner = checkCoordWall;
                        checkCoordWallJoiner.x -= (float) direction2.x / 2;
                        checkCoordWallJoiner.y -= (float) direction2.y / 2;

                        if(wallMap.Contains(checkCoordWall) && !wallJoinerMap.Contains(checkCoordWallJoiner))
                        {

                            CreateWallJoiner(new Vector3(checkCoordWallJoiner.x, 0, checkCoordWallJoiner.y), newWallTile);
                            wallJoinerMap.Add(checkCoordWallJoiner);
                        }
                    }
                }
            }
        }
    }

    public virtual void CreateWallJoiner(Vector3 spawnPos, GameObject parentWall)
    {

        GameObject newWallJoiner = Instantiate(wallJoiner, spawnPos, wallJoiner.transform.rotation);
        newWallJoiner.transform.LookAt(parentWall.transform);
        dum.AddGameObject(newWallJoiner);
    }
}
