using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    public Dictionary<Vector2Int, Tile> tileDict = new();
    public HashSet<Vector2Int> revealedTiles = new();
    [SerializeField] DungeonManager dum;
    [SerializeField] VisibilityManager visibilityManager;

    public void AddTile(Tile newTile)
    {

        if (newTile != null)
        {

            if (!tileDict.ContainsKey(newTile.coord))
            {

                tileDict.Add(newTile.coord, newTile);
            }
        }
    }

    public void RefreshLayout()
    {

        tileDict = new();
        revealedTiles = new();
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
        revealedTiles.Add(tileDict[tileCoord].coord);
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
            }
        }
    }
}
