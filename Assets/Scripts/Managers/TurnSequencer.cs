using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    
    public float aggroRange = 10;
    public bool gameplayHalted = false;
    public float baseWaitTime = 0.05f;
    public float incrementWaitTime = 0.05f;
    private bool actionTaken = false;
    private Queue<Vector3> playerMovementQueue = new();
    private HashSet<GameObject> movingEnemies = new();
    private PlayerCharacterSheet playerCharacter;
    private SpellCaster sc;
    private MoveToTarget pcMovement;
    private AttackAnimation pcAttackAnimation;
    private DungeonManager dum;
    private UIActiveManager uiam;
    private CombatManager cbm;
    private ClickManager cm;
    [SerializeField] private LevelGenerator levelGenerator;
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
            if(playerMovementQueue.Count > 0 && !pcMovement.IsMoving())
            {
                
                Vector3 nextMove = playerMovementQueue.Dequeue();
                playerCharacter.Move(nextMove, dum.occupiedlist);

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

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetTile.coord, dum.dungeonCoords, ignoredPoints: dum.GetOccupiedCoords());  
            
                        if(pathToDestination != null && pathToDestination.Count > 1)
                        {

                            if(dum.aggroEnemies.Count > 0)
                            {

                                if(!pcMovement.IsMoving())
                                {
                                
                                    playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), dum.occupiedlist);

                                }else
                                {

                                    return;
                                }

                            }else
                            {
                            
                                playerMovementQueue.Clear();

                                for (int i = 1; i < pathToDestination.Count; i++)
                                {

                                    Vector3 movePosition = new Vector3(pathToDestination[i].x, 0.1f, pathToDestination[i].y);
                                    playerMovementQueue.Enqueue(movePosition);
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
                            
                            playerMovementQueue.Clear();
                            targetExit.ExitLevel();                            
                            levelGenerator.NewLevel(levelGenerator.biomeDict[BiomeType.Catacomb]);
                            
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
                
                float waitTime = baseWaitTime;
                actionTaken = false;

                //give a turn to each aggroed enemy
                foreach(GameObject enemy in dum.aggroEnemies)
                {
                    
                    enemy.GetComponent<EnemyCharacterSheet>().AggroBehavior(playerCharacter, dum, cbm, waitTime);
                    waitTime += incrementWaitTime;
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

                        if(enemy.GetComponent<EnemyCharacterSheet>().OnAggro(dum, cbm))
                        {

                            dum.aggroEnemies.Add(enemy);

                            //automated walking via buffer is halted when an enemy sees you
                            playerMovementQueue.Clear();
                        }
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