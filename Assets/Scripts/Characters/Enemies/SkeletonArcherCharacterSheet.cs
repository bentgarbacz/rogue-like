using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonArcherCharacterSheet : EnemyCharacterSheet
{

    int attackCooldown = 0;
    int range = 5;
    ProjectileType projectile = ProjectileType.Arrow;

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

        dropTable = "Skeleton";
        title = "Skeleton Archer";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
    }

    public override void AggroBehavior()
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }

        List<Dictionary<Vector2Int, float>> mapsOfInterest = new(){djm.playerMap, djm.npcMap};
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

                bool attackResult = cbm.AddProjectileAttack(this.gameObject, targetEntity, range, minDamage, maxDamage, speed, projectile);
                
                //Ranged attack initates if target is within range
                if(attackResult)
                {

                    attackCooldown = 3;
                    return;
                }

                Debug.Log(targetEntity.GetComponent<CharacterSheet>().title);
            }

        }

        Wander();
        attackCooldown -= 1;
    }
}

