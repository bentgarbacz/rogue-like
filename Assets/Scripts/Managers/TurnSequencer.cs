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
    private Queue<Vector2Int> playerMovementQueue = new();
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
    [SerializeField] private TileManager tileManager;
    [SerializeField] private VisibilityManager visibilityManager;
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
        if (!cbm.fighting && !gameplayHalted)
        {
            //If you have not reached the last node of your path 
            //and you are not currently moving to a node, move to the next node
            if (playerMovementQueue.Count > 0 && !pcMovement.IsMoving())
            {

                playerCharacter.Move(playerMovementQueue.Dequeue(), dum.occupiedlist);
                UpkeepEffects();
                actionTaken = true;

                //Process a turn if:
                //left mouse was pressed
                //mouse is not over a blocking UI element
                //you are not in the middle of an attack animation
                //some other action has not paused regular gameplay by setting gameplayHalted to true
            }
            else if (mouse.leftButton.wasPressedThisFrame && uiam.IsPointerOverUI() == false && !pcAttackAnimation.IsAttacking() && !gameplayHalted)
            {

                GameObject target = cm.GetObject();

                //execute action if actionable object was clicked
                if (target != null && uiam.IsPointerOverUI() == false)
                {

                    Tile targetTile = target.GetComponent<Tile>();
                    CharacterSheet targetCharacter = target.GetComponent<CharacterSheet>();
                    Interactable targetInteractable = target.GetComponent<Interactable>();

                    uiam.CloseInventoryPanel();
                    uiam.CloseLootPanel();
                    uiam.CloseCharacterPanel();
                    uiam.HideAssignSpell();

                    playerMovementQueue.Clear();

                    if (targetTile != null)
                    {

                        if (!targetTile.IsActionable() ||
                            targetTile.coord == playerCharacter.coord ||
                            dum.occupiedlist.Contains(targetTile.coord))
                        {

                            return;
                        }

                        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetTile.coord, dum.dungeonCoords, ignoredPoints: dum.occupiedlist);

                        if (pathToDestination != null && pathToDestination.Count > 1)
                        {

                            if (dum.aggroEnemies.Count > 0)
                            {

                                MoveOneSpace(pathToDestination);

                            }
                            else
                            {

                                playerMovementQueue.Clear();

                                for (int i = 1; i < pathToDestination.Count; i++)
                                {

                                    playerMovementQueue.Enqueue(pathToDestination[i]);
                                }
                            }

                            return;
                        }
                    }

                    //initiate an attack on clicked enemy
                    //attack if adjacent to enemy
                    //move towards enemy if not adjacent
                    if (targetCharacter != null)
                    {

                        if (targetCharacter != playerCharacter)
                        {

                            playerCharacter.AttackCharacter(target);
                        }
                    }

                    if (targetInteractable != null)
                    {

                        //interact with interactable object
                        if (PathFinder.GetNeighbors(targetInteractable.coord, dum.dungeonCoords).Contains(playerCharacter.coord) || targetInteractable.coord == playerCharacter.coord)
                        {

                            if (!targetInteractable.Interact())
                            {

                                return;
                            }

                        }
                        else //move towards interactable
                        {

                            List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetInteractable.coord, dum.dungeonCoords);
                            MoveOneSpace(pathToDestination);
                            return;
                        }
                    }

                    actionTaken = true;
                }
            }

            //If the player has taken an action, give enemies a turn
            if (actionTaken && !gameplayHalted)
            {

                float waitTime = baseWaitTime;
                actionTaken = false;
                //give a turn to each aggroed enemy
                foreach (GameObject enemy in dum.aggroEnemies)
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
            if (dum.enemiesOnLookout)
            {

                foreach (GameObject enemy in dum.enemies)
                {

                    if (ShouldAggro(enemy))
                    {

                        if (enemy.GetComponent<EnemyCharacterSheet>().OnAggro(dum, cbm))
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
        visibilityManager.UpdateVisibilities();
    }

    private void MoveOneSpace(List<Vector2Int> pathToDestination)
    {

        playerMovementQueue.Clear();

        if (!pcMovement.IsMoving() && pathToDestination != null)
        {

            playerMovementQueue.Enqueue(pathToDestination[1]);

        }
        else
        {

            return;
        }
    }

    private bool ShouldAggro(GameObject enemy)
    {

        return aggroRange > Vector3.Distance(enemy.transform.position, dum.hero.transform.position) &&
            !dum.aggroEnemies.Contains(enemy) &&
            LineOfSight.HasLOS(enemy, dum.hero);
    }
}