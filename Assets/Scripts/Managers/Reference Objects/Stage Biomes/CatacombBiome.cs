using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatacombBiome : Biome
{

    [SerializeField] private GameObject catacombFloorTile;
    [SerializeField] private GameObject catacombFloorTileAlt1;
    [SerializeField] private GameObject catacombFloorTileAlt2;
    [SerializeField] private GameObject catacombFloorTileAlt3;
    [SerializeField] private GameObject catacombFloorTileAlt4;
    [SerializeField] private GameObject catacombWallTile;
    [SerializeField] private GameObject catacombEntrance;
    [SerializeField] private GameObject catacombExit;
    private readonly float exitSpawnPosVertOffset = 0.88f;

    public override void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG, DungeonManager dum)
    {

        GameObject newTile;

        if(spawnRNG <= 1000 && spawnRNG >= 997)
        {
            
            newTile = Instantiate(catacombFloorTileAlt1, spawnPos, catacombFloorTileAlt1.transform.rotation);

        }else if(spawnRNG <= 996 && spawnRNG >= 993)
        {

            newTile = Instantiate(catacombFloorTileAlt2, spawnPos, catacombFloorTileAlt2.transform.rotation);

        }else if(spawnRNG <= 992 && spawnRNG >= 989)
        {

            newTile = Instantiate(catacombFloorTileAlt3, spawnPos, catacombFloorTileAlt3.transform.rotation);

        }else if(spawnRNG <= 988 && spawnRNG >= 985)
        {

            newTile = Instantiate(catacombFloorTileAlt4, spawnPos, catacombFloorTileAlt4.transform.rotation);

        }else
        {

            newTile = Instantiate(catacombFloorTile, spawnPos, catacombFloorTile.transform.rotation);
        }
        
        dum.AddGameObject(newTile);
        newTile.GetComponent<Tile>().SetCoord(position);

    }

    public override void CreateEntranceTile(Vector3 spawnPos, Vector2Int position, DungeonManager dum)
    {

        GameObject newCatacombEntrance = Instantiate(catacombEntrance, spawnPos, catacombEntrance.transform.rotation);
        dum.AddGameObject(newCatacombEntrance);
        newCatacombEntrance.GetComponent<Tile>().SetCoord(position);
    }

    public override void CreateExitTile(Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum)
    {

        spawnPos.y -= exitSpawnPosVertOffset;
        GameObject newCatacombExit = Instantiate(catacombExit, spawnPos, catacombExit.transform.rotation);

        foreach(Vector2Int direction in Direction2D.cardinalDirectionsList)
        {

            if(path.Contains(position + direction))
            {
                
                newCatacombExit.transform.rotation = Quaternion.Euler(0, GameFunctions.DetermineRotation(newCatacombExit.transform.position, new Vector3(position.x + direction.x, 0, position.y + direction.y)), 0);
                break;
            }
        }

        dum.AddGameObject(newCatacombExit);
        newCatacombExit.GetComponent<Exit>().coord = position;
    }

    public override void CreateWallTile(Vector3 spawnPos, DungeonManager dum)
    {

        GameObject newWall = Instantiate(catacombWallTile, spawnPos, catacombWallTile.transform.rotation);
        newWall.GetComponent<Tile>().SetCoord(new Vector2Int((int)spawnPos.x, (int)spawnPos.z));
        dum.AddGameObject(newWall);
        //newWall.GetComponent<Renderer>().material.color = Color.gray;
        
    }
}
