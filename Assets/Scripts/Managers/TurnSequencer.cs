using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
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
    [SerializeField] private DijkstraMapManager djMapGenerator;
 
    void Start()
    {

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

        if (cbm.fighting || gameplayHalted)
        {

            return;
        }

        if (ProcessPlayerMovement())
        {

            return;
        }

        if (ProcessPlayerInput())
        {

            return;
        }

        if (actionTaken && !gameplayHalted)
        {

            ProcessEnemyTurns();
            cbm.CommenceCombat();
            UpkeepEffects();
            AggroNearbyEnemies();
        }        
    }

    private bool ProcessPlayerMovement()
    {

        if (playerMovementQueue.Count > 0 && !pcMovement.IsMoving())
        {

            playerCharacter.Move(playerMovementQueue.Dequeue(), dum.occupiedlist);
            actionTaken = true;
            return true;
        }

        return false;
    }

    private bool ProcessPlayerInput()
    {

        if (!Mouse.current.leftButton.wasPressedThisFrame || uiam.IsPointerOverUI() || pcAttackAnimation.IsAttacking() || gameplayHalted)
        {

            return false;
        }

        GameObject target = cm.GetObject();

        if (target == null || uiam.IsPointerOverUI())
        {

            return false;
        }
        string filePath = @"C:\Users\bentg\Downloads\map_output.txt";
        djMapGenerator.PrintMapToFile(djMapGenerator.CreateMapAboutObject(dum.hero, 100), filePath);

        Tile targetTile = target.GetComponent<Tile>();
        CharacterSheet targetCharacter = target.GetComponent<CharacterSheet>();
        Interactable targetInteractable = target.GetComponent<Interactable>();

        uiam.CloseInventoryPanel();
        uiam.CloseLootPanel();
        uiam.CloseCharacterPanel();
        uiam.HideAssignSpell();
        playerMovementQueue.Clear();

        if (HandleInteractable(targetInteractable))
        {

            return true;
        }

        if (HandleTile(targetTile))
        {

            return true;
        }

        if (HandleCharacter(targetCharacter))
        {

            return true;
        }

        return false;
    }

    private bool HandleInteractable(Interactable targetInteractable)
    {

        if (targetInteractable == null)
        {

            return false;
        }

        if (PathFinder.GetNeighbors(targetInteractable.loc.coord, dum.dungeonCoords).Contains(playerCharacter.loc.coord) ||
            targetInteractable.loc.coord == playerCharacter.loc.coord)
        {

            if (!targetInteractable.Interact())
            {

                return false;
            }

            actionTaken = true;
            return true;
            
        }else
        {

            List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.loc.coord, targetInteractable.loc.coord, dum.dungeonCoords);
            MoveOneSpace(pathToDestination);
            return true;
        }
    }

    private bool HandleTile(Tile targetTile)
    {

        if (targetTile == null)
        {

            return false;
        }

        if (!targetTile.IsActionable() ||
            targetTile.loc.coord == playerCharacter.loc.coord ||
            dum.occupiedlist.Contains(targetTile.loc.coord))
        {

            return false;
        }

        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.loc.coord, targetTile.loc.coord, dum.dungeonCoords, ignoredPoints: dum.occupiedlist);

        if (pathToDestination != null && pathToDestination.Count > 1)
        {

            if (dum.aggroEnemies.Count > 0)
            {

                MoveOneSpace(pathToDestination);
                return true;

            }
            else
            {

                playerMovementQueue.Clear();

                for (int i = 1; i < pathToDestination.Count; i++)
                {

                    playerMovementQueue.Enqueue(pathToDestination[i]);
                }
            }

            return true;
        }

        return false;
    }

    private bool HandleCharacter(CharacterSheet targetCharacter)
    {

        if (targetCharacter == null)
        {

            return false;
        }

        if (targetCharacter != playerCharacter)
        {

            playerCharacter.AttackCharacter(targetCharacter.gameObject);
        }

        actionTaken = true;
        return true;
    }

    private void ProcessEnemyTurns()
    {

        float waitTime = baseWaitTime;
        actionTaken = false;

        foreach (GameObject enemy in dum.aggroEnemies)
        {

            enemy.GetComponent<EnemyCharacterSheet>().AggroBehavior(playerCharacter, dum, cbm, waitTime);
            waitTime += incrementWaitTime;
        }
    }

    public void AggroNearbyEnemies()
    {

        if (!dum.enemiesOnLookout)
        {

            return;
        }

        foreach (GameObject enemy in dum.enemies)
        {

            EnemyCharacterSheet enemyCS = enemy.GetComponent<EnemyCharacterSheet>();

            if (!ShouldAggro(enemy, enemyCS))
            {

                continue;
            }

            if (enemyCS.OnAggro(dum, cbm))
            {

                dum.aggroEnemies.Add(enemy);
                playerMovementQueue.Clear();
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
    }

    private bool ShouldAggro(GameObject enemy, EnemyCharacterSheet ecs)
    {
        
        return !dum.aggroEnemies.Contains(enemy) &&
            ecs.aggroRange > Vector3.Distance(enemy.transform.position, dum.hero.transform.position) &&
            LineOfSight.HasLOS(enemy, dum.hero);
    }
}