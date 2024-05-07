using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public GameObject hero;
    public HashSet<Vector2Int> dungeonCoords;
    public Dictionary<string, List<Vector2Int>> cachedPathsDict;
    public HashSet<GameObject> enemies;
    public HashSet<GameObject> aggroEnemies = new HashSet<GameObject>();
    public HashSet<Vector3> occupiedlist;
    public List<Vector2Int> bufferedPath = new List<Vector2Int>();
    public float aggroRange = 10;
    private Character playerCharacter;
    private Mouse mouse;

    void Start()
    {

        hero = mapGen.GetComponent<MapGen>().hero;
        occupiedlist = mapGen.GetComponent<MapGen>().occupiedlist;
        enemies = mapGen.GetComponent<MapGen>().enemies;
        dungeonCoords = mapGen.GetComponent<MapGen>().path;
        cachedPathsDict = mapGen.GetComponent<MapGen>().cachedPathsDict;

        mouse = Mouse.current;
        playerCharacter = hero.GetComponent<Character>();
    }   
    
    void Update()
    {         

        //If you have not reached the last node of your path 
        //and you are not currently moving to a node, move to the next node
        if(bufferedPath.Count > 0 && playerCharacter.GetComponent<MoveToTarget>().GetRemainingDistance() == 0)
        {
            
            playerCharacter.Move(new Vector3(bufferedPath[0].x, 0.1f, bufferedPath[0].y), occupiedlist);
            bufferedPath.RemoveAt(0);

        }else if(mouse.leftButton.wasPressedThisFrame)
        {

            GameObject target = GetComponent<ClickManager>().getObject(mouse);   

            //move player character if tile is clicked
            if(target != null)
            {

                if(target.GetComponent<Tile>() && target.GetComponent<Tile>().coord != playerCharacter.coord)
                {

                    //Non-precached movement    
                    List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, target.GetComponent<Tile>().coord, dungeonCoords);   

                    //Precached movement
                    //PathKey pk = new PathKey(playerCharacter.coord, target.GetComponent<Tile>().coord);                                    
                    //List<Vector2Int> pathToDestination = cachedPathsDict[pk.key];
        
                    
                    playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);                
                    
                    //If there are no enemies alerted to your presence, automatically walk entire path to destiniation
                    if(aggroEnemies.Count == 0)
                    {
                        bufferedPath = new List<Vector2Int>(pathToDestination.Count);

                        foreach(Vector2Int node in pathToDestination)
                        {

                            bufferedPath.Add(new Vector2Int(node.x, node.y));
                        }

                        bufferedPath.RemoveAt(0);
                        bufferedPath.RemoveAt(0);
                    }              
                }

                //initiate an attack on clicked enemy
                //attack if adjacent to enemy
                //move towards enemy if not adjacent
                if(target.GetComponent<Character>() && target.GetComponent<Character>() != playerCharacter)
                {

                    Character targetCharacter = target.GetComponent<Character>();
                
                    if(PathFinder.GetNeighbors(targetCharacter.coord, dungeonCoords).Contains(playerCharacter.coord))
                    {

                        playerCharacter.Attack(targetCharacter);

                        //kills target of attack if it's health falls below 1
                        if(targetCharacter.health <= 0)
                        {
                            
                            aggroEnemies.Remove(target);
                            occupiedlist.Remove(targetCharacter.pos);
                            enemies.Remove(target);
                            target.GetComponent<TextPopup>().CleanUp();
                            Destroy(target);                                                                    
                        }

                    }else //move towards target
                    {

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetCharacter.coord, dungeonCoords);            
                        playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);
                    } 
                }

                //open container and examine loot
                if(target.GetComponent<Loot>())
                {

                    Loot targetContainer = target.GetComponent<Loot>();

                    //open loot container if it is a neighbor of player character or it is on top of player character
                    if(PathFinder.GetNeighbors(targetContainer.coord, dungeonCoords).Contains(playerCharacter.coord) || targetContainer.coord == playerCharacter.coord )
                    {
                        
                        targetContainer.OpenContainer();

                    }else //move towards container
                    {

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetContainer.coord, dungeonCoords);            
                        playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist); 
                    }                   
                }

                if(target.GetComponent<Exit>())
                { 

                    Tile targetExit = target.GetComponent<Tile>();

                    //erase current level and generate a new one
                    if(PathFinder.GetNeighbors(targetExit.coord, dungeonCoords).Contains(playerCharacter.coord) || targetExit.coord == playerCharacter.coord )
                    {

                        mapGen.GetComponent<MapGen>().CleanUp();
                        playerCharacter.Teleport(new Vector3(0, 0, 0), occupiedlist);
                        mapGen.GetComponent<MapGen>().NewLevel();
                        occupiedlist = mapGen.GetComponent<MapGen>().occupiedlist;
                        enemies = mapGen.GetComponent<MapGen>().enemies;
                        dungeonCoords = mapGen.GetComponent<MapGen>().path;
                        cachedPathsDict = mapGen.GetComponent<MapGen>().cachedPathsDict;

                    }else //move towards exit
                    {

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetExit.coord, dungeonCoords);            
                        playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist); 
                    }   
                }            

                //give a turn to each aggroed enemy
                foreach(GameObject enemy in aggroEnemies)
                {

                    Character nonPlayerCharacter = enemy.GetComponent<Character>();

                    //enemy attacks player character if they are in a neighboring tile
                    if(PathFinder.GetNeighbors(playerCharacter.coord, dungeonCoords).Contains(nonPlayerCharacter.coord))
                    {
                        nonPlayerCharacter.Attack(playerCharacter);

                        if(playerCharacter.health <= 0) //kills player if their health falls below 1
                        {
                            
                            Destroy(playerCharacter);
                            print("Game Over");                                            
                        }

                    }else //enemy moves towards player or attack if they are in a neighboring tile                 
                    {

                        List<Vector2Int> pathToPlayer = PathFinder.FindPath(nonPlayerCharacter.coord, playerCharacter.coord, dungeonCoords); 

                        //If enemy is not neighboring player character...
                        if(pathToPlayer.Count > 1)
                        {
                            
                            //...try to move towards player character...
                            if(!nonPlayerCharacter.Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), occupiedlist))
                            {                  
                                
                                //...if that spot is occupied then try to path to a tile adjascent to player character 
                                foreach(Vector2Int v in NeighborVals.allDirectionsList)
                                {

                                    List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(nonPlayerCharacter.coord, playerCharacter.coord + v, dungeonCoords);
                                    
                                    if(pathToPlayerPlusV != null)
                                    {

                                        if(nonPlayerCharacter.Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), occupiedlist))
                                        {

                                            break;
                                        }
                                    }
                                }        
                            } 
                        } 
                    }
                }                       
            }              
        }

        //aggro enemies within aggroRange units            
        foreach(GameObject enemy in enemies)
        {

            if(aggroRange > Vector3.Distance(enemy.transform.position, hero.transform.position) && !aggroEnemies.Contains(enemy))
            {

                aggroEnemies.Add(enemy);
                enemy.GetComponent<TextPopup>().CreatePopup(enemy.transform.position, 2, "!", Color.red);

                //automated walking via buffer is halted when an enemy sees you
                bufferedPath.Clear();
            }
        }
    } 
}
