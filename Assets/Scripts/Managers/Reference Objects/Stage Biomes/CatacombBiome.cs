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
    private readonly List<NPCType> possibleEnemyTypes = new()
    {

        NPCType.Skeleton,
        NPCType.SkeletonArcher,
        NPCType.Slime
    };

    public override void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG, DungeonManager dum)
    {

        GameObject newTile;

        if(spawnRNG <= 1000 && spawnRNG >= 800)
        {
            
            newTile = Instantiate(catacombFloorTileAlt1, spawnPos, catacombFloorTileAlt1.transform.rotation);

        }else if(spawnRNG <= 800 && spawnRNG >= 600)
        {

            newTile = Instantiate(catacombFloorTileAlt2, spawnPos, catacombFloorTileAlt2.transform.rotation);

        }else if(spawnRNG <= 600 && spawnRNG >= 400)
        {

            newTile = Instantiate(catacombFloorTileAlt3, spawnPos, catacombFloorTileAlt3.transform.rotation);

        }else if(spawnRNG <= 400 && spawnRNG >= 200)
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

    public override bool GenerateLevel(Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum, NPCGenerator npcGen)
    {

        int width = 50;
        int height = 50;

        HashSet<Vector2Int> grid = PathFinder.GenerateBlankGrid(width, height);
        List<Room> rooms = BSPGenerator.Generate(width, height, 20, 15);

        Dictionary<Vector2Int, float> costDict = new();

        foreach(Vector2Int point in grid)
        {

            costDict.Add(point, 0f);
        }

        List<Vector2Int> roomPositions = new();
        
        foreach(Room room in rooms)
        {

            roomPositions.Add(room.position);

            foreach(Vector2Int coord in room.GetCoordinates())
            {
                
                //Add to set of navigable coordinates in the dungeon
                path.Add(coord);

                //Spawn a tile in the game world
                Vector3 spawnPos = new(coord.x, 0, coord.y);
                CreateTile(spawnPos, coord, Random.Range(0, 1001), dum);

                //discourage room tiles for when we generate hallways
                costDict[coord] = 5f;
            }

            foreach(Vector2Int coord in room.GetPerimeterCoordinates())
            {

                //REALLY discourage wall tiles of rooms for when we generate hallways
                costDict[coord] = 10;
            }
        }

        List<Triangle> triangles = DelaunayTriangulation.GenerateTriangulation(roomPositions);

        HashSet<Edge> allEdges = DelaunayTriangulation.GetEdgesFromTriangles(triangles);

        HashSet<Edge> chosenEdges = MinimumSpanningTree.CreateMSTExtraEdges(allEdges, 0.125f);


        foreach(Edge edge in chosenEdges)
        {

            Vector2Int navigateCoord1 = edge.coord1;
            Vector2Int navigateCoord2 = edge.coord2;

            foreach(Room room in rooms)
            {

                if(room.position == navigateCoord1)
                {

                    navigateCoord1 = room.GetRandomCoordinate();

                }else if(room.position == navigateCoord2)
                {

                    navigateCoord2 = room.GetRandomCoordinate();

                }
            }            

            List<Vector2Int> hallwayCoords = PathFinder.FindPath(navigateCoord1, navigateCoord2, grid, false, costDict);

            foreach(Vector2Int coord in hallwayCoords)
            {

                if(!path.Contains(coord))
                {

                    //Add to set of navigable coordinates in the dungeon
                    path.Add(coord);

                    //Spawn tile in the game world
                    Vector3 spawnPos = new(coord.x, 0, coord.y);
                    CreateTile(spawnPos, coord, Random.Range(0, 1001), dum);

                    //encourage existing hallway tiles to be reused
                    costDict[coord] = -5f;
                }
            }
        }

        //Generate entrance and place dum.hero there
        Room entranceRoom = rooms[0]; // Select the first room as the entrance room
        Vector2Int entranceCoord = entranceRoom.GetRandomCoordinate();
        Vector3 entrancePos = new Vector3(entranceCoord.x, 0, entranceCoord.y);
        dum.DeleteTile(entranceCoord);
        // Create the entrance tile
        CreateEntranceTile(entrancePos, entranceCoord, dum);

        // Place the hero at the entrance
        dum.hero.transform.position = entrancePos;

        foreach(Vector2Int coord in PathFinder.GetNeighbors(entranceCoord, path))
        {

            PlayerCharacterSheet pc = dum.hero.GetComponent<PlayerCharacterSheet>();
            Vector3 movePos = new(coord.x, 0f, coord.y);

            if(pc.Move(movePos, dum.occupiedlist))
            {

                break;
            }
        }

        //Generate exit in the last room in rooms
        Room exitRoom = rooms[^1];
        Vector2Int exitCoord = exitRoom.GetRandomCoordinate();
        Vector3 exitPos = new(exitCoord.x, 0, exitCoord.y);

        dum.DeleteTile(exitCoord);

        // Create the exit tile
        CreateExitTile(exitPos, exitCoord, path, dum);

        //spawn 0 - 3 chests in every room
        foreach (Room room in rooms)
        {

            int chestCount = Random.Range(0, 4); // Randomly decide how many chests to spawn (0 to 3)
            HashSet<Vector2Int> usedChestCoords = new HashSet<Vector2Int>();

            for (int i = 0; i < chestCount; i++)
            {
                
                Vector2Int chestCoord;

                // Find a valid coordinate that hasn't been used yet
                do
                {
                    chestCoord = room.GetRandomCoordinate();
                } while (usedChestCoords.Contains(chestCoord));

                usedChestCoords.Add(chestCoord); // Mark the coordinate as used
                Vector3 chestPos = new Vector3(chestCoord.x, 0, chestCoord.y);

                // Spawn the chest at the chosen position
                npcGen.CreateChest(chestPos, dum);
            }
        }

        //spawn 0 - 3 enemies in each room
        for (int roomIndex = 1; roomIndex < rooms.Count; roomIndex++) // Start from room[1] to exclude room[0]
        {
            Room room = rooms[roomIndex];
            int enemyCount = Random.Range(0, 4); // Randomly decide how many enemies to spawn (0 to 3)
            HashSet<Vector2Int> usedEnemyCoords = new HashSet<Vector2Int>(); // Track used coordinates for enemies

            for (int i = 0; i < enemyCount; i++)
            {
                Vector2Int enemyCoord;

                // Find a valid coordinate that hasn't been used yet
                do
                {
                    enemyCoord = room.GetRandomCoordinate();
                } while (usedEnemyCoords.Contains(enemyCoord));

                usedEnemyCoords.Add(enemyCoord); // Mark the coordinate as used
                Vector3 enemyPos = new Vector3(enemyCoord.x, 0, enemyCoord.y);

                // Spawn the enemy at the chosen position
                npcGen.CreateNPC(possibleEnemyTypes[ Random.Range(0, 3) ], enemyPos, dum);
            }
        }

       return true;
    }
}
