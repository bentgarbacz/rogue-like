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

    public override bool GenerateLevel(Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum, NPCGenerator npcGen)
    {

        List<Room> rooms = BSPGenerator.Generate(100, 100, 20, 15);

        List<Vector2Int> points = new List<Vector2Int>();
        
        foreach(Room room in rooms)
        {

            points.Add(room.Position);

            foreach(Vector2Int coord in room.GetCoordinates())
            {

                path.Add(coord);
                Vector3 spawnPos = new(coord.x, 0, coord.y);
                CreateTile(spawnPos, coord, 1, dum);
            }
        }

        List<Triangle> triangles = DelaunayTriangulation.GenerateTriangulation(points);

        List<Edge> allEdges = DelaunayTriangulation.GetEdgesFromTriangles(triangles);

        List<Edge> edgesMST = MinimumSpanningTree.CreateMST(allEdges);

        List<Edge> edges = MinimumSpanningTree.AddExtraEdges(edgesMST, allEdges, 0.125f);


        foreach(Edge edge in edges)
        {
            Vector2Int navigateCoord1 = edge.coord1;
            Vector2Int navigateCoord2 = edge.coord2;

            foreach(Room room in rooms)
            {

                if(room.Position == navigateCoord1)
                {

                    navigateCoord1 = room.GetRandomCoordinate();

                }else if(room.Position == navigateCoord2)
                {

                    navigateCoord2 = room.GetRandomCoordinate();

                }
            }

            List<Vector2Int> hallwayCoords = PathFinder.FindPath(navigateCoord1, navigateCoord2, PathFinder.GenerateBlankGrid(100, 100), false);

            foreach(Vector2Int coord in hallwayCoords)
            {

                if(!path.Contains(coord))
                {

                    path.Add(coord);
                    Vector3 spawnPos = new(coord.x, 0, coord.y);
                    CreateTile(spawnPos, coord, 1, dum);
                }
            }
        }

        //Generate entrance and place dum.hero there
        Room entranceRoom = rooms[0]; // Select the first room as the entrance room (or choose based on your logic)
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
            Vector3 movePos = new Vector3(coord.x, 0f, coord.y);


            if(pc.Move(movePos, dum.occupiedlist))
            {

                break;
            }
        }

        //Generate exit in the last room in rooms
        Room exitRoom = rooms[rooms.Count - 1]; // Select the last room as the exit room
        Vector2Int exitCoord = exitRoom.GetRandomCoordinate();
        Vector3 exitPos = new Vector3(exitCoord.x, 0, exitCoord.y);

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
                int randomChoice = Random.Range(0, 3); // Randomly choose 0, 1, or 2

                switch (randomChoice)
                {
                    case 0:
                        npcGen.CreateNPC(NPCType.Skeleton, enemyPos, dum);
                        break;
                    case 1:
                        npcGen.CreateNPC(NPCType.SkeletonArcher, enemyPos, dum);
                        break;
                    case 2:
                        npcGen.CreateNPC(NPCType.Slime, enemyPos, dum);
                        break;
                }
            }
        }

       return true;
    }
}
