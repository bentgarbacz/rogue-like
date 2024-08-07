using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public float aggroRange = 10;
    public bool gameplayHalted = false;
    private bool actionTaken = false;
    private PlayerCharacter playerCharacter;
    private DungeonManager dum;
    private UIActiveManager uiam;
    private CombatManager cbm;
    private Mouse mouse;

    void Start()
    {
 
        mouse = Mouse.current;
        GameObject managers = GameObject.Find("System Managers");
        dum = managers.GetComponent<DungeonManager>();
        uiam = managers.GetComponent<UIActiveManager>();
        cbm = managers.GetComponent<CombatManager>();
        playerCharacter = dum.hero.GetComponent<PlayerCharacter>();
    }   
    
    void Update()
    {         

        //Halt game logic if combat is taking place
        if(!cbm.fighting)
        {
            //If you have not reached the last node of your path 
            //and you are not currently moving to a node, move to the next node
            if(dum.bufferedPath.Count > 0 && !playerCharacter.GetComponent<MoveToTarget>().moving)
            {
                
                playerCharacter.Move(new Vector3(dum.bufferedPath[0].x, 0.1f, dum.bufferedPath[0].y), dum.occupiedlist);
                dum.bufferedPath.RemoveAt(0);

                UpkeepEffects();

            //Process a turn if:
            //left mouse was pressed
            //mouse is not over a blocking UI element
            //you are not in the middle of an attack animation
            //some other action has not paused regular gameplay by setting gameplayHalted to true
            }else if(mouse.leftButton.wasPressedThisFrame && uiam.IsPointerOverUI() == false && !dum.hero.GetComponent<AttackAnimation>().IsAttacking() && !gameplayHalted)
            {

                GameObject target = GetComponent<ClickManager>().GetObject();   

                //execute action if actionable object was clicked
                if(target != null && uiam.IsPointerOverUI() == false)
                {

                    uiam.CloseInventoryPanel();
                    uiam.CloseLootPanel();
                    uiam.CloseCharacterPanel();
                    uiam.HideAssignSpell();

                    actionTaken = true;

                    UpkeepEffects();

                    if(target.GetComponent<Tile>() && target.GetComponent<Tile>().coord != playerCharacter.coord)
                    {

                        //Non-precached movement    
                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, target.GetComponent<Tile>().coord, dum.dungeonCoords);  

                        //Precached movement
                        //PathKey pk = new PathKey(playerCharacter.coord, target.GetComponent<Tile>().coord);                                    
                        //List<Vector2Int> pathToDestination = cachedPathsDict[pk.key];
            
                        if(pathToDestination != null)
                        {

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
                    }

                    //initiate an attack on clicked enemy
                    //attack if adjacent to enemy
                    //move towards enemy if not adjacent
                    if(target.GetComponent<Character>() && target.GetComponent<Character>() != playerCharacter)
                    {

                        cbm.AddToCombatBuffer(dum.hero, target);
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
                }
            }

            //If the player has taken an action, give enemies a turn
            if(actionTaken && !gameplayHalted)
            {
                
                actionTaken = false;

                //give a turn to each aggroed enemy
                foreach(GameObject enemy in new HashSet<GameObject>(dum.aggroEnemies))
                {
                    
                    enemy.GetComponent<Enemy>().AggroBehavior(playerCharacter, dum, cbm);
                }

                //start combat for the turn
                cbm.CommenceCombat();
            }
            

            //aggro enemies within aggroRange units            
            foreach(GameObject enemy in dum.enemies)
            {

                if(aggroRange > Vector3.Distance(enemy.transform.position, dum.hero.transform.position) && !dum.aggroEnemies.Contains(enemy) && LineOfSight.HasLOS(enemy, dum.hero))
                {

                    dum.aggroEnemies.Add(enemy);
                    enemy.GetComponent<TextNotificationManager>().CreateNotificationOrder(enemy.transform.position, 2, "!", Color.red);

                    //automated walking via buffer is halted when an enemy sees you
                    dum.bufferedPath.Clear();
                }
            }
        }
    }

    public void SignalAction()
    {

        actionTaken = true;
    }

    private void UpkeepEffects()
    {

        dum.TriggerStatusEffects();
        playerCharacter.BecomeHungrier();
        playerCharacter.DecrementCooldowns();
    }
}