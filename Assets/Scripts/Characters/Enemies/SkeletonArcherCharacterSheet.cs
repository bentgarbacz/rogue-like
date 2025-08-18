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

    public override void AggroBehavior(float waitTime)
    {

        if(attackCooldown == 0)
        {

            //Ranged attack initates if target is within range
            if(cbm.AddProjectileAttack(this.gameObject, dum.hero, range, minDamage, maxDamage, speed, projectile))
            {

                attackCooldown = 3;

            }else //move towards target if not within range
            {
                
                List<Vector2Int> pathToPlayer = PathFinder.FindPath(loc.coord, dum.playerCharacter.loc.coord, dum.dungeonCoords);                     
                Move(pathToPlayer[1], waitTime);
            }

        }else
        {

            Wander(waitTime);
            attackCooldown -= 1;
        }
    }
}

