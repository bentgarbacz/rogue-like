using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public float aggroRange = 10;
    private PlayerCharacter playerCharacter;
    private DungeonManager dum;
    private InventoryManager im;
    private UIActiveManager uiam;
    private Mouse mouse;

    void Start()
    {
 
        mouse = Mouse.current;
        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
        im = GameObject.Find("CanvasHUD").GetComponent<InventoryManager>();
        uiam = GameObject.Find("System Managers").GetComponent<UIActiveManager>();
        playerCharacter = dum.hero.GetComponent<PlayerCharacter>();
    }   
    
    void Update()
    {         

        //If you have not reached the last node of your path 
        //and you are not currently moving to a node, move to the next node
        if(dum.bufferedPath.Count > 0 && playerCharacter.GetComponent<MoveToTarget>().GetRemainingDistance() == 0)
        {
            
            playerCharacter.Move(new Vector3(dum.bufferedPath[0].x, 0.1f, dum.bufferedPath[0].y), dum.occupiedlist);
            dum.bufferedPath.RemoveAt(0);
            playerCharacter.BecomeHungrier();

        }else if(mouse.leftButton.wasPressedThisFrame)
        {

            GameObject target = GetComponent<ClickManager>().getObject(mouse);   

            //execute action if actionable object was clicked
            if(target != null && uiam.IsPointerOverUI() == false)
            {

                uiam.CloseInventoryPanel();
                uiam.CloseLootPanel();

                playerCharacter.BecomeHungrier();

                if(target.GetComponent<Tile>() && target.GetComponent<Tile>().coord != playerCharacter.coord)
                {

                    //Non-precached movement    
                    List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, target.GetComponent<Tile>().coord, dum.dungeonCoords);   

                    //Precached movement
                    //PathKey pk = new PathKey(playerCharacter.coord, target.GetComponent<Tile>().coord);                                    
                    //List<Vector2Int> pathToDestination = cachedPathsDict[pk.key];
        
                    
                    playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist);                
                    
                    //If there are no enemies alerted to your presence, automatically walk entire path to destiniation
                    if(dum.aggroEnemies.Count == 0)
                    {
                        dum.bufferedPath = new List<Vector2Int>(pathToDestination.Count);

                        foreach(Vector2Int node in pathToDestination)
                        {

                            dum.bufferedPath.Add(new Vector2Int(node.x, node.y));
                        }

                        dum.bufferedPath.RemoveRange(0, 2);
                    }              
                }

                //initiate an attack on clicked enemy
                //attack if adjacent to enemy
                //move towards enemy if not adjacent
                if(target.GetComponent<Character>() && target.GetComponent<Character>() != playerCharacter)
                {

                    Character targetCharacter = target.GetComponent<Character>();
                
                    if(PathFinder.GetNeighbors(targetCharacter.coord, dum.dungeonCoords).Contains(playerCharacter.coord))
                    {

                        playerCharacter.Attack(targetCharacter);

                        //kills target of attack if it's health falls below 1
                        if(targetCharacter.health <= 0)
                        {
                            
                            dum.Smite(target, targetCharacter.pos);                                                                    
                        }

                    }else //move towards target
                    {

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetCharacter.coord, dum.dungeonCoords);            
                        playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist);
                    }
                }

                //open container and examine loot
                if(target.GetComponent<Loot>())
                {

                    Loot targetContainer = target.GetComponent<Loot>();

                    //open loot container if it is a neighbor of player character or it is on top of player character
                    if(PathFinder.GetNeighbors(targetContainer.coord, dum.dungeonCoords).Contains(playerCharacter.coord) || targetContainer.coord == playerCharacter.coord )
                    {
                        
                        targetContainer.OpenContainer(target);

                    }else //move towards container
                    {

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetContainer.coord, dum.dungeonCoords);            
                        playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist); 
                    }
                }

                if(target.GetComponent<Exit>())
                { 

                    Exit targetExit = target.GetComponent<Exit>();

                    //erase current level and generate a new one
                    if(PathFinder.GetNeighbors(targetExit.coord, dum.dungeonCoords).Contains(playerCharacter.coord) || targetExit.coord == playerCharacter.coord )
                    {

                        targetExit.ExitLevel();
                        
                        mapGen.GetComponent<MapGenerator>().NewLevel();
                        
                    }else //move towards exit
                    {

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetExit.coord, dum.dungeonCoords);            
                        playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist); 
                    }   
                }

                //give a turn to each aggroed enemy
                foreach(GameObject enemy in dum.aggroEnemies)
                {

                    Character nonPlayerCharacter = enemy.GetComponent<Character>();

                    //enemy attacks player character if they are in a neighboring tile
                    if(PathFinder.GetNeighbors(playerCharacter.coord, dum.dungeonCoords).Contains(nonPlayerCharacter.coord))
                    {
                        nonPlayerCharacter.Attack(playerCharacter);

                        if(playerCharacter.health <= 0) //kills player if their health falls below 1
                        {
                            
                            Destroy(playerCharacter);
                            print("Game Over");                                            
                        }

                    }else //enemy moves towards player or attack if they are in a neighboring tile                 
                    {

                        List<Vector2Int> pathToPlayer = PathFinder.FindPath(nonPlayerCharacter.coord, playerCharacter.coord, dum.dungeonCoords); 

                        //If enemy is not neighboring player character...
                        if(pathToPlayer.Count > 1)
                        {
                            
                            //...try to move towards player character...
                            if(!nonPlayerCharacter.Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), dum.occupiedlist))
                            {                  
                                
                                //...if that spot is occupied then try to path to a tile adjascent to player character 
                                foreach(Vector2Int v in NeighborVals.allDirectionsList)
                                {

                                    List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(nonPlayerCharacter.coord, playerCharacter.coord + v, dum.dungeonCoords);
                                    
                                    if(pathToPlayerPlusV != null)
                                    {

                                        if(nonPlayerCharacter.Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), dum.occupiedlist))
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
        foreach(GameObject enemy in dum.enemies)
        {

            if(aggroRange > Vector3.Distance(enemy.transform.position, dum.hero.transform.position) && !dum.aggroEnemies.Contains(enemy))
            {

                dum.aggroEnemies.Add(enemy);
                enemy.GetComponent<TextPopup>().CreatePopup(enemy.transform.position, 2, "!", Color.red);

                //automated walking via buffer is halted when an enemy sees you
                dum.bufferedPath.Clear();
            }
        }
    }
}
