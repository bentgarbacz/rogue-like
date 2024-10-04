using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileReferences : MonoBehaviour
{

    [SerializeField] private GameObject wallJoiner;
    private Dictionary<BiomeType, Biome> biomeDict;

    void Start()
    {

        biomeDict = new()
        {
            {BiomeType.Catacomb, GetComponent<CatacombBiome>()}
        };
    }

    public void CreateTile(BiomeType biomeType, Vector3 spawnPos, Vector2Int position, int spawnRNG, DungeonManager dum)
    {

        biomeDict[biomeType].CreateTile(spawnPos, position, spawnRNG, dum);
    }

    public void CreateEntranceTile(BiomeType biomeType, Vector3 spawnPos, Vector2Int position, DungeonManager dum)
    {

        biomeDict[biomeType].CreateEntranceTile(spawnPos, position, dum);
    }

    public void CreateExitTile(BiomeType biomeType, Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum)
    {

        biomeDict[biomeType].CreateExitTile(spawnPos, position, path, dum);
    }

    public void CreateWallTile(BiomeType biomeType, Vector3 spawnPos, DungeonManager dum)
    {

        biomeDict[biomeType].CreateWallTile(spawnPos, dum);
    }

    public void CreateWallJoiner(Vector3 spawnPos, DungeonManager dum)
    {

        GameObject newWallJoiner = Instantiate(wallJoiner, spawnPos, wallJoiner.transform.rotation);
        dum.AddGameObject(newWallJoiner);
    }
}

public enum BiomeType
{

    Catacomb
}