using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public Dictionary<Vector2Int, Tile> tileDict = new();
    public HashSet<Vector2Int> revealedTiles = new();
    public HashSet<Vector2Int> doorCoords = new();
    [SerializeField] DungeonManager dum;
    [SerializeField] VisibilityManager visibilityManager;
    [SerializeField] MiniMapManager mm;

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

        if (tileDict.ContainsKey(coord))
        {

            dum.dungeonSpecificGameObjects.Remove(tileDict[coord].gameObject);
            Destroy(tileDict[coord].gameObject);
            tileDict.Remove(coord);
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

            if (tileDict[tileCoord].state == false && LineOfSight.HasLOS(dum.hero, tileDict[tileCoord].gameObject))
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
