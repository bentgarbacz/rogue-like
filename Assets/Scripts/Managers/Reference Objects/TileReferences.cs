using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileReferences : MonoBehaviour
{

    [SerializeField] private GameObject wallJoiner;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject floorTileAlt1;
    [SerializeField] private GameObject floorTileAlt2;
    [SerializeField] private GameObject floorTileAlt3;
    [SerializeField] private GameObject floorTileAlt4;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject enterance;
    [SerializeField] private GameObject exit;
    private readonly float exitSpawnPosVertOffset = 0.88f;

    public void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG, DungeonManager dum)
    {

        GameObject newTile;

        if(spawnRNG <= 1000 && spawnRNG >= 997)
        {
            
            newTile = Instantiate(floorTileAlt1, spawnPos, floorTileAlt1.transform.rotation);

        }else if(spawnRNG <= 996 && spawnRNG >= 993)
        {

            newTile = Instantiate(floorTileAlt2, spawnPos, floorTileAlt2.transform.rotation);

        }else if(spawnRNG <= 992 && spawnRNG >= 989)
        {

            newTile = Instantiate(floorTileAlt3, spawnPos, floorTileAlt3.transform.rotation);

        }else if(spawnRNG <= 988 && spawnRNG >= 985)
        {

            newTile = Instantiate(floorTileAlt4, spawnPos, floorTileAlt4.transform.rotation);

        }else
        {

            newTile = Instantiate(floorTile, spawnPos, floorTile.transform.rotation);
        }
        
        dum.AddGameObject(newTile);
        newTile.GetComponent<Tile>().SetCoord(position);

    }

    public void CreateEnteranceTile(Vector3 spawnPos, Vector2Int position, DungeonManager dum)
    {

        GameObject newEnterance = Instantiate(enterance, spawnPos, enterance.transform.rotation);
        dum.AddGameObject(newEnterance);
        newEnterance.GetComponent<Tile>().SetCoord(position);
    }

    public void CreateExitTile(Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum)
    {

        spawnPos.y -= exitSpawnPosVertOffset;
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
    }

    public void CreateWallTile(Vector3 spawnPos, DungeonManager dum)
    {

        GameObject newWall = Instantiate(wallTile, spawnPos, wallTile.transform.rotation);
        newWall.GetComponent<Tile>().SetCoord(new Vector2Int((int)spawnPos.x, (int)spawnPos.z));
        dum.AddGameObject(newWall);
        newWall.GetComponent<Renderer>().material.color = Color.gray;
        
    }

    public void CreateWallJoiner(Vector3 spawnPos, DungeonManager dum)
    {

        GameObject newWallJoiner = Instantiate(wallJoiner, spawnPos, wallJoiner.transform.rotation);
        dum.AddGameObject(newWallJoiner);
    }
}