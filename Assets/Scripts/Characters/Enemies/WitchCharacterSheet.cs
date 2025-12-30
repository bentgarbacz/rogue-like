using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCharacterSheet : EnemyCharacterSheet
{

    private int attackCooldown = 0;
    private int teleportCooldown = 0;
    private int range = 5;
    private ProjectileType projectile = ProjectileType.MagicMissile;
    private AudioClip teleportClip;
    [SerializeField] private Levitating levitating;

    public override void Awake()
    {

        base.Awake();
        maxHealth = 8;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Witch";
        title = "Witch";

        attackClip = Resources.Load<AudioClip>("Sounds/Mystical");
        teleportClip = Resources.Load<AudioClip>("Sounds/Teleport");

        levitating.StartLevitating();
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

        if(attackCooldown == 0 && targetCoord != new Vector2Int(int.MaxValue, int.MaxValue))
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

                bool attackResult = cbm.AddProjectileAttack(this.gameObject, targetEntity, range, minDamage, maxDamage, speed, projectile);
                
                //Ranged attack initates if target is within range
                if(attackResult)
                {

                    attackCooldown = 3;

                }else //move towards target if not within range
                {
                    
                    List<Vector2Int> pathToTarget = PathFinder.FindPath(loc.coord, targetCoord, tileMgr.levelCoords);      
                    movementManager.AddMovement(this, pathToTarget[1]);               
                }
            }

        }else
        {
            if(teleportCooldown == 0)
            {

                audioSource.PlayOneShot(teleportClip);
                teleportCooldown = 3;

            }else
            {
               
                Flee(djm.playerAndNpcMap);
            }

            attackCooldown -= 1;
            teleportCooldown -= 1;
        }
    }
}

