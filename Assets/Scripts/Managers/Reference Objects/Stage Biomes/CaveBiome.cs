using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBiome : Biome
{

    [SerializeField] private GameObject caveFloorTileAlt1;
    [SerializeField] private GameObject caveFloorTileAlt2;
    [SerializeField] private GameObject caveFloorTileAlt3;
    [SerializeField] private GameObject caveFloorTileAlt4;
    [SerializeField] private GameObject caveFloorTileAlt5;
    [SerializeField] private GameObject caveWallTile1;
    [SerializeField] private GameObject caveWallTile2;
    [SerializeField] private GameObject caveWallTile3;
    [SerializeField] private GameObject caveWallTile4;
    [SerializeField] private GameObject caveWallTile5;
    [SerializeField] private GameObject caveEntrance;
    [SerializeField] private GameObject caveExit;
    private readonly float exitSpawnPosVertOffset = 0.88f;
    private int walkLength = 100;
    private int repeatWalks = 100;
    public List<NPCType> possibleEnemyTypes = new();

    public override void CreateTile(Vector3 spawnPos, Vector2Int position, int spawnRNG, DungeonManager dum)
    {

        GameObject newTile;

        if(spawnRNG <= 1000 && spawnRNG >= 800)
        {
            
            newTile = Instantiate(caveFloorTileAlt1, spawnPos, caveFloorTileAlt1.transform.rotation);

        }else if(spawnRNG <= 800 && spawnRNG >= 600)
        {

            newTile = Instantiate(caveFloorTileAlt2, spawnPos, caveFloorTileAlt2.transform.rotation);

        }else if(spawnRNG <= 600 && spawnRNG >= 400)
        {

            newTile = Instantiate(caveFloorTileAlt3, spawnPos, caveFloorTileAlt3.transform.rotation);

        }else if(spawnRNG <= 400 && spawnRNG >= 200)
        {

            newTile = Instantiate(caveFloorTileAlt4, spawnPos, caveFloorTileAlt4.transform.rotation);

        }else
        {

            newTile = Instantiate(caveFloorTileAlt5, spawnPos, caveFloorTileAlt5.transform.rotation);
        }
        
        dum.AddGameObject(newTile);
        newTile.GetComponent<Tile>().SetCoord(position);

    }

    public override void CreateEntranceTile(Vector3 spawnPos, Vector2Int position, DungeonManager dum)
    {

        GameObject newCatacombEntrance = Instantiate(caveEntrance, spawnPos, caveEntrance.transform.rotation);
        dum.AddGameObject(newCatacombEntrance);
        newCatacombEntrance.GetComponent<Tile>().SetCoord(position);
    }

    public override void CreateExitTile(Vector3 spawnPos, Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum)
    {

        GameObject newCatacombExit = Instantiate(caveExit, spawnPos, caveExit.transform.rotation);
        
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

        GameObject newWall;
        int spawnRNG = Random.Range(1, 1001);



        if(spawnRNG <= 1000 && spawnRNG >= 800)
        {
            
            newWall = Instantiate(caveWallTile1, spawnPos, caveWallTile1.transform.rotation);

        }else if(spawnRNG <= 800 && spawnRNG >= 600)
        {

            newWall = Instantiate(caveWallTile2, spawnPos, caveWallTile2.transform.rotation);

        }else if(spawnRNG <= 600 && spawnRNG >= 400)
        {

            newWall = Instantiate(caveWallTile3, spawnPos, caveWallTile3.transform.rotation);

        }else if(spawnRNG <= 400 && spawnRNG >= 200)
        {

            newWall = Instantiate(caveWallTile4, spawnPos, caveWallTile4.transform.rotation);

        }else
        {

            newWall = Instantiate(caveWallTile5, spawnPos, caveWallTile5.transform.rotation);
        }

        newWall.GetComponent<Tile>().SetCoord(new Vector2Int((int)spawnPos.x, (int)spawnPos.z));
        dum.AddGameObject(newWall);      
    }

    public override bool GenerateLevel(Vector2Int position, HashSet<Vector2Int> path, DungeonManager dum, NPCGenerator npcGen)
    {
       
       bool exitSpawned = false;

        for(int i = 0; i < repeatWalks; i++)
        {

            for(int j = 0; j < walkLength; j++)
            {
                
                if( !path.Contains(position) )
                {

                    Vector3 spawnPos = new(position.x, 0, position.y);
                    int spawnRNG = UnityEngine.Random.Range(0, 1000);

                    //spawn enterance on first tile
                    if(i == 0 && j == 0)
                    {

                        CreateEntranceTile(spawnPos, position, dum);

                    //potentially spawn exit after a certain number of steps
                    }else if(i == repeatWalks - 1 && j > walkLength / 2 && exitSpawned == false)
                    {

                        CreateExitTile(spawnPos, position, path, dum);
                        exitSpawned = true;
                    
                    //Otherwise spawn a normal tile
                    }else
                    {

                        CreateTile(spawnPos, position, spawnRNG, dum);                       
                    }

                    //move player to first tile generated
                    if(i == 0 && j == 1)
                    {

                        dum.hero.GetComponent<CharacterSheet>().Move(spawnPos + new Vector3(0f, 0.1f, 0f), dum.occupiedlist);   

                    }else if(Mathf.Abs(position.x) > 5 || Mathf.Abs(position.y) > 5){

                        if(spawnRNG >= 0 && spawnRNG <= 2)
                        {

                            npcGen.CreateChest(spawnPos, dum);

                        }else if(spawnRNG >= 3 && spawnRNG <= 4)
                        {

                            npcGen.CreateNPC(NPCType.Spider, spawnPos, dum);

                        }else if(spawnRNG >= 5 && spawnRNG <= 6)
                        {

                            npcGen.CreateNPC(NPCType.Spider, spawnPos, dum);

                        }else if(spawnRNG >= 7 && spawnRNG <= 8)
                        {

                            npcGen.CreateNPC(NPCType.Spider, spawnPos, dum);
                        }
                    }
                }

                path.Add(position);
                position += Direction2D.GetRandomDirection();
            }
        }

        if(!exitSpawned)
        {

            return false;
        }

        return true;
    }
}
