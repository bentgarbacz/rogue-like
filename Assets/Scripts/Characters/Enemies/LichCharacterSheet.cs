using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichCharacterSheet : EnemyCharacterSheet
{

    private int attackCooldown = 0;
    private int summonCooldown = 0;
    private int range = 6;
    private ProjectileType projectile = ProjectileType.MagicMissile;
    private NPCGenerator npcGen;
    private AudioClip summonClip;
    [SerializeField] private Levitating levitating;

    public override void Awake()
    {

        base.Awake();
        maxHealth = 12;
        accuracy = 100;
        minDamage = 2;
        maxDamage = 4;
        level = 5;
        speed = 9;
        evasion = 30;

        characterHealth.InitHealth(maxHealth);
        levitating.StartLevitating();

        dropTable = DropTableType.Witch;
        title = "Lich";

        attackClip = Resources.Load<AudioClip>("Sounds/Mystical");
        summonClip = Resources.Load<AudioClip>("Sounds/Teleport");

        npcGen = GameObject.Find("Map Generator").GetComponent<NPCGenerator>();
    }

    public override void AggroBehavior()
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }

        List<Dictionary<Vector2Int, float>> mapsOfInterest = new(){djm.npcMap, djm.playerMap};
        Vector2Int targetCoord = GetRangedTarget(loc.coord, mapsOfInterest);

        if(attackCooldown <= 0 && targetCoord != new Vector2Int(int.MaxValue, int.MaxValue))
        {

            GameObject targetEntity = null;

            foreach (GameObject entity in tileMgr.GetTile(targetCoord).entitiesOnTile)
            {

                if (entity != null && entity.GetComponent<CharacterSheet>() != null)
                {

                    targetEntity = entity;
                }
            }

            if (targetEntity != null)
            {

                bool inRange = combatSeq.CheckProjectileAttackValidity(this.gameObject, targetEntity, range);

                //Ranged attack initiates if target is within range
                if(inRange)
                {

                    Attack attack = new(this.gameObject, targetEntity, minDamage, maxDamage, speed, projectile);
                    combatSeq.AddAttack(attack);

                    attackCooldown = 3;

                }else //move towards target if not within range
                {
                    
                    List<Vector2Int> pathToTarget = PathFinder.FindPath(loc.coord, targetCoord, tileMgr.levelCoords);      
                    movementManager.AddMovement(this, pathToTarget[1]);               
                }
            }

        }else
        {
            
            if(summonCooldown <= 0 && Random.value < 0.4f)
            {

                if(AttemptSummonSkeleton())
                {

                    summonCooldown = 5;
                }
            }
            else
            {

                Flee(djm.playerAndNpcMap);
            }

            attackCooldown -= 1;
            summonCooldown -= 1;
        }
    }

    private bool AttemptSummonSkeleton()
    {

        // Try to find an empty adjacent tile to summon a skeleton
        foreach (Vector2Int direction in Direction2D.DirectionsList())
        {

            Vector2Int summonCoord = loc.coord + direction;

            if (tileMgr.levelCoords.Contains(summonCoord) && !tileMgr.occupiedlist.Contains(summonCoord))
            {

                Vector3 summonPos = new(summonCoord.x, transform.position.y, summonCoord.y);
                GameObject skeleton = npcGen.CreateEnemy(NPCType.Skull, summonPos);

                if (skeleton != null)
                {

                    audioSource.PlayOneShot(summonClip);
                    return true;
                }
            }
        }

        return false;
    }
}
