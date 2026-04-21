using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    private bool turnLock = false;
    private int turnLockCount = 0;
    private bool actionTaken = false;
    private Queue<Vector2Int> playerMovementQueue = new();
    private PlayerCharacterSheet playerCharacter;
    private SpellCaster spellCaster;
    private MoveToTarget pcMovement;
    [SerializeField] private EntityManager entityMgr;
    [SerializeField] private TileManager tileMgr;
    [SerializeField] private UIActiveManager uiam;
    [SerializeField] private CombatSequencer combatSeq;
    [SerializeField] private ClickManager clickMgr;
    [SerializeField] private NameplateManager namePlateMgr;
    [SerializeField] private MiniMapManager minimapMgr;
    [SerializeField] private VisibilityManager visibilityMgr;
    [SerializeField] private DijkstraMapManager djMapMgr;
    [SerializeField] private NPCMovementManager movementManager;

    void Start()
    {
       
        clickMgr = GetComponent<ClickManager>();
        playerCharacter = entityMgr.hero.GetComponent<PlayerCharacterSheet>();
        spellCaster = entityMgr.hero.GetComponent<SpellCaster>();
        pcMovement = playerCharacter.GetComponent<MoveToTarget>();
    }

    void Update()
    {

        if (ProcessPlayerInput())
        {

            return;
        }

        if (combatSeq.fighting || turnLock)
        {

            return;
        }

        if (actionTaken)
        {

            actionTaken = false;

            djMapMgr.PopulatePlayerMap();
            djMapMgr.UpdateCombinedMapPlayerAndNPC();
            ProcessEntityTurns(entityMgr.aggroEnemies);
            djMapMgr.PopulateEnemyMap();
            ProcessEntityTurns(entityMgr.npcs);
            djMapMgr.PopulateNPCMap();
            
            combatSeq.CommenceCombat();
            UpkeepEffects();
            AggroNearbyEnemies();
        }

        if (ProcessMovement())
        {

            return;
        }
    }

    private bool ProcessMovement()
    {

        if (playerMovementQueue.Count > 0 && !pcMovement.IsMoving())
        {

            playerCharacter.Move(playerMovementQueue.Dequeue());
            //actionTaken = true;
            return true;
        }

        movementManager.ProcessMovement();
        minimapMgr.UpdateDynamicIcons();

        return false;
    }

    private bool ProcessPlayerInput()
    {

        if (!Mouse.current.leftButton.wasPressedThisFrame || uiam.IsPointerOverUI() || combatSeq.fighting || (turnLock && !IsPlayerMoving()))
        {

            return false;
        }

        GameObject target = clickMgr.GetObject();

        if (target == null || uiam.IsPointerOverUI())
        {

            return false;
        }

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

        if (PathFinder.GetNeighbors(targetInteractable.loc.coord, tileMgr.levelCoords).Contains(playerCharacter.loc.coord) ||
            targetInteractable.loc.coord == playerCharacter.loc.coord)
        {

            if (!targetInteractable.Interact())
            {

                return false;
            }

            actionTaken = true;
            return true;

        }
        else
        {

            List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.loc.coord, targetInteractable.loc.coord, tileMgr.levelCoords);
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
            tileMgr.occupiedlist.Contains(targetTile.loc.coord))
        {

            return false;
        }

        List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.loc.coord, targetTile.loc.coord, tileMgr.levelCoords, ignoredPoints: tileMgr.occupiedlist);

        if (pathToDestination != null && pathToDestination.Count > 1)
        {

            if (entityMgr.aggroEnemies.Count > 0)
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

    private void ProcessEntityTurns(HashSet<GameObject> entities)
    {

        HashSet<GameObject> entitiesCopy = new(entities);

        foreach (GameObject entity in entitiesCopy)
        {

            entity.GetComponent<NpcCharacterSheet>().AggroBehavior();
        }
    }

    public void AggroNearbyEnemies()
    {

        if (!entityMgr.enemiesOnLookout)
        {

            return;
        }

        foreach (GameObject enemy in entityMgr.enemies)
        {

            EnemyCharacterSheet enemyCS = enemy.GetComponent<EnemyCharacterSheet>();

            if (!ShouldAggro(enemy, enemyCS))
            {

                continue;
            }

            if (enemyCS.OnAggro())
            {

                entityMgr.aggroEnemies.Add(enemy);
                playerMovementQueue.Clear();
            }
        }
    }
    
    public void IncrementTurnLock(int count = 1)
    {
        
        if(count < 1)
        {
            
            return;
        }

        turnLockCount += count;

        if(turnLockCount > 0)
        {
            
            turnLock = true;
        }
    }

    public void DecrementTurnLock(int count = 1)
    {
        
        if(count < 1)
        {
            
            return;
        }

        turnLockCount = Mathf.Max(turnLockCount - count, 0);

        if(turnLockCount == 0)
        {
            
            turnLock = false;
        }
    }

    private bool IsPlayerMoving()
    {
        return playerMovementQueue.Count > 0 || pcMovement.IsMoving();
    }

    public bool TurnLockState()
    {
        
        return turnLock;
    }

    //Only use this when player character dies
    public void DeadLock()
    {
        
        turnLock = true;
        turnLockCount = int.MaxValue;
    }

    public void SignalAction()
    {

        actionTaken = true;
    }

    public void UpkeepEffects()
    {

        entityMgr.TriggerStatusEffects();
        playerCharacter.BecomeHungrier();
        playerCharacter.DecrementCooldowns();
        spellCaster.UpdateSpellSlots();
        minimapMgr.UpdateDynamicIcons();
        namePlateMgr.IncrementDisplayTimer();
        visibilityMgr.UpdateVisibilities();
    }

    private void MoveOneSpace(List<Vector2Int> pathToDestination)
    {

        playerMovementQueue.Clear();

        if (!pcMovement.IsMoving() && pathToDestination != null)
        {

            playerMovementQueue.Enqueue(pathToDestination[1]);
        }
    }

    private bool ShouldAggro(GameObject enemy, EnemyCharacterSheet ecs = null)
    {

        if(ecs == null)
        {

            ecs = enemy.GetComponent<EnemyCharacterSheet>();
        }

        if(ecs == null)
        {
            
            return false;
        }

        bool isAggroed = entityMgr.aggroEnemies.Contains(enemy);
        bool inPlayerRange = ecs.aggroRange >= djMapMgr.GetPlayerMapValue(ecs.loc.coord);
        bool inNpcRange = ecs.aggroRange >= djMapMgr.GetNpcMapValue(ecs.loc.coord);
        bool seesTarget = LineOfSight.HasLOS(enemy, entityMgr.hero);

        if (!seesTarget)
        {

            foreach (GameObject entity in entityMgr.npcs)
            {

                if (LineOfSight.HasLOS(enemy, entity))
                {

                    seesTarget = true;
                    break;
                }
            }
        }

        return !isAggroed && seesTarget && (inPlayerRange || inNpcRange);
    }
}