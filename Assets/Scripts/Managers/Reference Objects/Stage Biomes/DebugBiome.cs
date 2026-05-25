using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBiome : Biome
{
    [SerializeField] private GameObject debugFloorTile;
    [SerializeField] private GameObject debugWallTile;
    [SerializeField] private GameObject debugEntrance;
    [SerializeField] private GameObject debugExit;

    public override void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG)
    {
        GameObject newTile = Instantiate(debugFloorTile, spawnPos, debugFloorTile.transform.rotation);
        newTile.GetComponent<Tile>().SetCoord(position);
        entityMgr.AddGameObject(newTile);
    }

    public override void CreateEntranceTile(Vector3 spawnPos, Vector2Int position)
    {
        GameObject newEntrance = Instantiate(debugEntrance, spawnPos, debugEntrance.transform.rotation);
        newEntrance.GetComponent<Tile>().SetCoord(position);
        entityMgr.AddGameObject(newEntrance);
    }

    public override void CreateExitTile(Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> dungeonCoords)
    {
        GameObject newExit = Instantiate(debugExit, spawnPos, debugExit.transform.rotation);
        newExit.GetComponent<ObjectVisibility>().Initialize();
        
        foreach (Vector2Int direction in Direction2D.cardinalDirectionsList)
        {
            if (dungeonCoords.Contains(position + direction))
            {
                newExit.transform.rotation = Quaternion.Euler(0, GameFunctions.DetermineRotation(newExit.transform.position, new Vector3(position.x + direction.x, 0, position.y + direction.y)), 0);
                break;
            }
        }

        newExit.GetComponent<Exit>().loc.coord = position;
        entityMgr.AddGameObject(newExit);
    }

    public override GameObject CreateWallTile(Vector3 spawnPos)
    {
        GameObject newWall = Instantiate(debugWallTile, spawnPos, debugWallTile.transform.rotation);
        newWall.GetComponent<Tile>().SetCoord(new Vector2Int((int)spawnPos.x, (int)spawnPos.z));
        entityMgr.AddGameObject(newWall);
        return newWall;
    }

    public override Vector2Int GenerateLevel(HashSet<Vector2Int> dungeonCoords)
    {
        // Create a simple grid of tiles
        int gridSize = 20;
        Vector2Int startPosition = new(-gridSize / 2, -gridSize / 2);
        Vector2Int firstTileCoord = startPosition + new Vector2Int(1, 1);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int position = startPosition + new Vector2Int(x, y);
                Vector3 spawnPos = new(position.x, 0, position.y);

                // Spawn entrance
                if (x == 0 && y == 0)
                {
                    //CreateEntranceTile(spawnPos, position);
                    //dungeonCoords.Add(position);
                }
                // Spawn main exit
                else if (x == gridSize - 1 && y == gridSize - 1)
                {
                    //CreateExitTile(spawnPos, position, dungeonCoords);
                    //dungeonCoords.Add(position);
                }
                // Regular floor tiles
                else
                {
                    CreateTile(spawnPos, position, 0);
                    dungeonCoords.Add(position);
                }
            }
        }

        // Spawn all NPC types in a grid pattern
        //SpawnAllNPCs(startPosition, gridSize);

        // Spawn biome exits
        //SpawnBiomeExits(dungeonCoords, startPosition, gridSize);

        // Generate walls around the dungeon
        GenerateWalls(dungeonCoords);

        return firstTileCoord;
    }

    private void SpawnBiomeExits(HashSet<Vector2Int> dungeonCoords, Vector2Int startPosition, int gridSize)
    {
        BiomeType[] otherBiomes = new[]
        {
            BiomeType.Catacomb,
            BiomeType.Cave
        };

        for (int i = 0; i < otherBiomes.Length; i++)
        {
            Vector2Int exitCoord = startPosition + new Vector2Int(gridSize / 2, 2 + i);
            Vector3 spawnPos = new(exitCoord.x, 0, exitCoord.y);

            // Only create floor tile if this coordinate hasn't been used yet
            if (!dungeonCoords.Contains(exitCoord))
            {
                CreateTile(spawnPos, exitCoord, 0);
                dungeonCoords.Add(exitCoord);
            }

            // Create the exit
            GameObject newExit = Instantiate(debugExit, spawnPos, debugExit.transform.rotation);
            newExit.GetComponent<ObjectVisibility>().Initialize();
            Exit exitComponent = newExit.GetComponent<Exit>();
            exitComponent.loc.coord = exitCoord;
            exitComponent.SetTargetBiome(otherBiomes[i]);
            entityMgr.AddGameObject(newExit);
        }
    }

    private void SpawnAllNPCs(Vector2Int startPosition, int gridSize)
    {
        NPCType[] allNPCTypes = new[]
        {
            NPCType.Chest,
            NPCType.MimicChest,
            NPCType.Skeleton,
            NPCType.SkeletalRemains,
            NPCType.SkeletonArcher,
            NPCType.Goblin,
            NPCType.Rat,
            NPCType.Slime,
            NPCType.Witch,
            NPCType.GoatMan,
            NPCType.Spider,
            NPCType.Skull,
            NPCType.Floater,
            NPCType.StoneGolem,
            NPCType.Lich,
            NPCType.Trap,
            NPCType.Mimic
        };

        int gridWidth = 6; // Number of NPCs per row in the spawn grid

        for (int i = 0; i < allNPCTypes.Length; i++)
        {
            int row = i / gridWidth;
            int col = i % gridWidth;
            Vector2Int spawnCoord = startPosition + new Vector2Int(col + 2, row + 2);
            Vector3 spawnPos = new(spawnCoord.x, 0, spawnCoord.y);

            if (allNPCTypes[i] == NPCType.Chest)
            {
                npcGen.CreateChest(spawnPos, 0);
            }
            else if (allNPCTypes[i] == NPCType.MimicChest)
            {
                npcGen.CreateChest(spawnPos, 100);
            }
            else
            {
                npcGen.CreateEnemy(allNPCTypes[i], spawnPos);
            }
        }
    }
}
