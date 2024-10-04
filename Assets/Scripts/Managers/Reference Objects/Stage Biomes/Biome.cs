using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    
    public virtual void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG, DungeonManager dum)
    {

        return;
    }

    public virtual void CreateEntranceTile(Vector3 spawnPos, Vector2Int position, DungeonManager dum)
    {

        return;
    }

    public virtual void CreateExitTile(Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum)
    {

        return;
    }

    public virtual void CreateWallTile(Vector3 spawnPos, DungeonManager dum)
    {

        return;
    }
}
