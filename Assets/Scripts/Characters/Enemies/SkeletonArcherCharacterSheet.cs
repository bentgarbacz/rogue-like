using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonArcherCharacterSheet : EnemyCharacterSheet
{

    int attackCooldown = 0;
    int range = 5;
    ProjectileType projectile = ProjectileType.Arrow;

    public override void Start()
    {
        
        base.Start();
        maxHealth = 8;
        health = maxHealth;
        accuracy = 100;
        minDamage = 1;
        maxDamage = 3;
        level = 3;
        speed = 11;
        evasion = 50;

        dropTable = "Skeleton";
        title = "Skeleton Archer";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
    }

    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime)
    {

        if(attackCooldown == 0)
        {

            //Ranged attack initates if target is within range
            if(cbm.AddProjectileAttack(this.gameObject, dum.hero, range, minDamage, maxDamage, speed, projectile))
            {

                attackCooldown = 3;

            }else //move towards target if not within range
            {
                
                List<Vector2Int> pathToPlayer = PathFinder.FindPath(coord, playerCharacter.coord, dum.dungeonCoords);                     
                Move(pathToPlayer[1], dum.occupiedlist, waitTime);
            }

        }else
        {

            //randomly walk while reloading ranged attack
            Vector2Int randomDirection = new(coord.x + Direction2D.GetRandomDirection().x, coord.y + Direction2D.GetRandomDirection().y);

            if(dum.dungeonCoords.Contains(randomDirection))
            {

                Move(randomDirection, dum.occupiedlist, waitTime);
            }

            attackCooldown -= 1;
        }
    }
}

