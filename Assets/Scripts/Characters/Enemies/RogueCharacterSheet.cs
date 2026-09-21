using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueCharacterSheet : EnemyCharacterSheet
{

    private int abilityActionCooldown = 0;
    private int range = 5;
    private ProjectileType poisonKnifeProjectile = ProjectileType.Arrow;
    private float poisonChance = 0.8f;
    private int poisonDuration = 3;
    private int poisonDamage = 2;
    private NPCGenerator npcGenerator;

    public override void Awake()
    {
        
        base.Awake();
        stats.maxHealth = 12;
        stats.accuracy = 95;
        stats.minDamage = 2;
        stats.maxDamage = 5;
        level = 4;
        stats.speed = 18;
        stats.evasion = 120;

        dropTable = DropTableType.Goblin;
        title = "Rogue";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");

        // Get NPCGenerator from EntityManager
        npcGenerator = GameObject.Find("System Managers").GetComponent<EntityManager>().GetComponentInChildren<NPCGenerator>();
    }

    public override void AggroBehavior()
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }

        // Handle ability cooldown
        if(abilityActionCooldown > 0)
        {

            abilityActionCooldown -= 1;
            
            // Normal double melee attack during cooldown
            List<Dictionary<Vector2Int, float>> mapsOfInterest = new(){djm.npcMap, djm.playerMap};
            Vector2Int targetCoord = GetNeighborTileOfMostInterest(loc.coord, mapsOfInterest);
            
            if(Mathf.Min(djm.GetNpcMapValue(targetCoord), djm.GetPlayerMapValue(targetCoord)) == 0)
            {

                // Double attack - attack twice in one turn
                AttackEntity(targetCoord);
                AttackEntity(targetCoord);

            }else
            {

                movementManager.AddMovement(this, targetCoord);
            }

            return;
        }

        // Use ability unpredictably (33% chance for each of three actions)
        int abilityChoice = Random.Range(0, 3);

        List<Dictionary<Vector2Int, float>> maps = new(){djm.playerMap, djm.npcMap};
        Vector2Int targetCoord2 = GetRangedTarget(loc.coord, maps);

        if(targetCoord2 == new Vector2Int(int.MaxValue, int.MaxValue))
        {

            // No target in range, do normal movement
            List<Dictionary<Vector2Int, float>> mapsOfInterest2 = new(){djm.npcMap, djm.playerMap};
            Vector2Int moveTarget = GetNeighborTileOfMostInterest(loc.coord, mapsOfInterest2);
            movementManager.AddMovement(this, moveTarget);
            return;
        }

        // Try to use ability
        switch(abilityChoice)
        {

            case 0: // Throw trap
                if(ThrowTrap(targetCoord2))
                {
                    abilityActionCooldown = 2;
                }
                break;

            case 1: // Throw poison knife
                if(ThrowPoisonKnife(targetCoord2))
                {
                    abilityActionCooldown = 1;
                }
                break;

            case 2: // Throw poison knife (same as case 1 for unpredictability)
                if(ThrowPoisonKnife(targetCoord2))
                {
                    abilityActionCooldown = 1;
                }
                break;
        }
    }

    private bool ThrowTrap(Vector2Int targetCoord)
    {

        // Check if target is in range
        bool inRange = combatSeq.CheckProjectileAttackValidity(this.gameObject, null, range);

        if(!inRange)
        {

            return false;
        }

        // Create trap at target location
        if(npcGenerator != null)
        {

            Vector3 trapSpawnPos = new(targetCoord.x, 0, targetCoord.y);
            npcGenerator.CreateTrap(trapSpawnPos);
            return true;
        }

        return false;
    }

    private bool ThrowPoisonKnife(Vector2Int targetCoord)
    {

        GameObject targetEntity = null;

        foreach (GameObject entity in tileMgr.GetTile(targetCoord).entitiesOnTile)
        {

            if (entity != null && entity.GetComponent<CharacterSheet>() != null)
            {

                targetEntity = entity;
            }
        }

        if (targetEntity == null)
        {

            return false;
        }

        bool inRange = combatSeq.CheckProjectileAttackValidity(this.gameObject, targetEntity, range);

        if(!inRange)
        {

            return false;
        }

        // Create poison knife attack (using arrow projectile with poison modifier)
        Attack attack = new(this.gameObject, targetEntity, stats.minDamage, stats.maxDamage, stats.speed, poisonKnifeProjectile);
        Poison poison = new(targetEntity.GetComponent<CharacterSheet>(), poisonDuration, poisonDamage);

        attack.AttachModifier(poison, poisonChance);
        combatSeq.AddAttack(attack);

        return true;
    }

    protected override bool AttackEntity(Vector2Int coord)
    {

        GameObject targetEntity = null;

        foreach (GameObject entity in tileMgr.GetTile(coord).entitiesOnTile)
        {

            if (entity != null && entity.GetComponent<CharacterSheet>() != null)
            {

                targetEntity = entity;
            }
        }

        if (targetEntity == null || !combatSeq.CheckMeleeAttackValidity(this.gameObject, targetEntity))
        {

            return false;
        }

        // Melee attack with poison chance
        Attack attack = new(this.gameObject, targetEntity, stats.minDamage, stats.maxDamage, stats.speed);
        Poison poison = new(targetEntity.GetComponent<CharacterSheet>(), poisonDuration, poisonDamage);

        attack.AttachModifier(poison, poisonChance * 0.5f); // Lower poison chance on melee
        combatSeq.AddAttack(attack);

        return true;
    }
}
