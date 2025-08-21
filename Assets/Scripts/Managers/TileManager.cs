using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<Vector2Int> occupiedlist = new();
    public Dictionary<Vector2Int, Tile> tileDict = new();
    public HashSet<Vector2Int> revealedTiles = new();
    public HashSet<Vector2Int> doorCoords = new();
    [SerializeField] private EntityManager entityMgr;
    [SerializeField] private VisibilityManager visibilityManager;
    [SerializeField] private MiniMapManager mm;

    public void AddTile(Tile newTile)
    {

        if (newTile != null)
        {

            if (!tileDict.ContainsKey(newTile.loc.coord))
            {

                tileDict.Add(newTile.loc.coord, newTile);
            }
        }
    }

    public void AddDoor(Door door)
    {

        if (door != null)
        {

            doorCoords.Add(door.loc.coord);
        }
    }

    public void RefreshLayout()
    {

        tileDict = new();
        revealedTiles = new();
        doorCoords = new();
    }

    public void DeleteTile(Vector2Int coord)
    {

        if (!tileDict.ContainsKey(coord))
        {

            return;
        }

        entityMgr.entitiesInLevel.Remove(tileDict[coord].gameObject);
        Destroy(tileDict[coord].gameObject);
        tileDict.Remove(coord);
    }

    public Tile GetTile(Vector2Int coord)
    {
        
        if (tileDict.ContainsKey(coord))
        {

            return tileDict[coord];
        }

        //Debug.Log(coord + " Does not exist in current level");

        return null;
    }

    public void MoveEntity(GameObject entity, Vector2Int startCoord, Vector2Int endCoord)
    {

        Tile startTile = GetTile(startCoord);
        Tile endTile = GetTile(endCoord);

        occupiedlist.Add(endCoord);
        occupiedlist.Remove(startCoord);

        if (startTile)
        {

            startTile.RemoveEntity(entity);
        }

        if (endTile)
        {

            endTile.AddEntity(entity);
        }
    }

    public void RevealTiles(HashSet<Vector2Int> tileCoords)
    {

        foreach (Vector2Int tileCoord in tileCoords)
        {

            if (!tileDict.ContainsKey(tileCoord))
            {

                continue;
            }

            if (tileDict[tileCoord].state == false && LineOfSight.HasLOS(entityMgr.hero, tileDict[tileCoord].gameObject))
            {

                RevealTile(tileCoord);

                RevealNeighboringDoors(tileCoord);

                if (!tileDict[tileCoord].IsActionable())
                {

                    continue;
                }

                RevealNeighboringWalls(tileCoord);
            }
        }

        visibilityManager.UpdateVisibilities();
    }

    public void RevealTile(Vector2Int tileCoord)
    {

        tileDict[tileCoord].SetState(true);
        mm.RevealTile(tileCoord);
        revealedTiles.Add(tileCoord);
    }

    public void RevealNeighboringWalls(Vector2Int tileCoord)
    {

        List<Vector2Int> directions = NeighborVals.cardinalList;

        foreach (Vector2Int d in directions)
        {

            Vector2Int checkCoord = new(tileCoord.x + d.x, tileCoord.y + d.y);

            if (!tileDict.ContainsKey(checkCoord))
            {

                continue;
            }

            if (!tileDict[checkCoord].IsActionable() && tileDict[checkCoord].state == false)
            {

                tileDict[checkCoord].SetState(true);
                mm.RevealTile(checkCoord);
            }
        }
    }

    public void RevealNeighboringDoors(Vector2Int tileCoord)
    {

        List<Vector2Int> directions = NeighborVals.cardinalList;

        foreach (Vector2Int d in directions)
        {

            Vector2Int checkCoord = new(tileCoord.x + d.x, tileCoord.y + d.y);

            if (doorCoords.Contains(checkCoord) && !revealedTiles.Contains(checkCoord))
            {

                RevealTile(checkCoord);
            }
        }
    }

    public void RevealAllTiles()
    {

        foreach(Vector2Int coord in tileDict.Keys)
        {

            if (!revealedTiles.Contains(coord))
            {

                RevealTile(coord);
            }
        }
    }
}
