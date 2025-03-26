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
    private PlayerCharacterSheet playerCharacter;
    private SpellCaster sc;
    private MoveToTarget pcMovement;
    private AttackAnimation pcAttackAnimation;
    private DungeonManager dum;
    private UIActiveManager uiam;
    private CombatManager cbm;
    private ClickManager cm;
    [SerializeField] private NameplateManager npm;
    [SerializeField] private MiniMapManager miniMapManager;
    private Mouse mouse;

    void Start()
    {
 
        mouse = Mouse.current;
        GameObject managers = GameObject.Find("System Managers");
        dum = managers.GetComponent<DungeonManager>();
        uiam = managers.GetComponent<UIActiveManager>();
        cbm = managers.GetComponent<CombatManager>();
        cm = GetComponent<ClickManager>();
        playerCharacter = dum.hero.GetComponent<PlayerCharacterSheet>();
        sc = dum.hero.GetComponent<SpellCaster>();
        pcMovement = playerCharacter.GetComponent<MoveToTarget>();
        pcAttackAnimation = dum.hero.GetComponent<AttackAnimation>();
    }   
    
    void Update()
    {         

        //Halt game logic if combat is taking place
        if(!cbm.fighting && !gameplayHalted)
        {
            //If you have not reached the last node of your path 
            //and you are not currently moving to a node, move to the next node
            if(dum.bufferedPath.Count > 0 && !pcMovement.moving)
            {
                
                playerCharacter.Move(new Vector3(dum.bufferedPath[0].x, 0.1f, dum.bufferedPath[0].y), dum.occupiedlist);
                dum.bufferedPath.RemoveAt(0);

                UpkeepEffects();

            //Process a turn if:
            //left mouse was pressed
            //mouse is not over a blocking UI element
            //you are not in the middle of an attack animation
            //some other action has not paused regular gameplay by setting gameplayHalted to true
            }else if(mouse.leftButton.wasPressedThisFrame && uiam.IsPointerOverUI() == false && !pcAttackAnimation.IsAttacking() && !gameplayHalted)
            {

                GameObject target = cm.GetObject();

                //execute action if actionable object was clicked
                if(target != null && uiam.IsPointerOverUI() == false)
                {

                    Tile targetTile = target.GetComponent<Tile>();
                    CharacterSheet targetCharacter = target.GetComponent<CharacterSheet>();
                    Loot targetLoot = target.GetComponent<Loot>();
                    Exit targetExit = target.GetComponent<Exit>();

                    uiam.CloseInventoryPanel();
                    uiam.CloseLootPanel();
                    uiam.CloseCharacterPanel();
                    uiam.HideAssignSpell();

                    if(targetTile != null)
                    {

                        if(!targetTile.IsActionable() || targetTile.coord == playerCharacter.coord)
                        {

                            return;
                        }

                        if(targetTile.coord != playerCharacter.coord)
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
                    }

                    //initiate an attack on clicked enemy
                    //attack if adjacent to enemy
                    //move towards enemy if not adjacent
                    if(targetCharacter != null)
                    {
                        
                        if(targetCharacter != playerCharacter)
                        {

                            //cbm.AddToCombatBuffer(dum.hero, target);
                            playerCharacter.AttackCharacter(target);
                        }
                    }

                    //open container and examine loot
                    if(targetLoot != null)
                    {

                        //open loot container if it is a neighbor of player character or it is on top of player character
                        if(PathFinder.GetNeighbors(targetLoot.coord, dum.dungeonCoords).Contains(playerCharacter.coord) || targetLoot.coord == playerCharacter.coord )
                        {
                            
                            targetLoot.OpenContainer();

                        }else //move towards container
                        {

                            List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetLoot.coord, dum.dungeonCoords);            
                            playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist); 
                        }
                    }

                    if(targetExit != null)
                    {

                        //erase current level and generate a new one
                        if(PathFinder.GetNeighbors(targetExit.coord, dum.dungeonCoords).Contains(playerCharacter.coord) || targetExit.coord == playerCharacter.coord )
                        {

                            targetExit.ExitLevel();                            
                            mapGen.GetComponent<LevelGenerator>().NewLevel();
                            
                        }else //move towards exit
                        {

                            List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetExit.coord, dum.dungeonCoords);            
                            playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist); 
                        }   
                    }

                    actionTaken = true;
                }
            }

            //If the player has taken an action, give enemies a turn
            if(actionTaken && !gameplayHalted)
            {
                
                actionTaken = false;

                //give a turn to each aggroed enemy
                foreach(GameObject enemy in dum.aggroEnemies)
                {
                    
                    enemy.GetComponent<EnemyCharacterSheet>().AggroBehavior(playerCharacter, dum, cbm);
                }

                //start combat for the turn
                cbm.CommenceCombat();

                //Perform upkeep effects for the turn
                UpkeepEffects();
            }
            

            //aggro enemies within aggroRange units 
            if(dum.enemiesOnLookout)
            {
        
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
        sc.UpdateSpellSlots();
        miniMapManager.UpdateDynamicIcons();
        npm.IncrementDisplayTimer();
    }
}