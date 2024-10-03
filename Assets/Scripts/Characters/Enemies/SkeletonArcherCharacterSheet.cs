using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherCharacterSheet : EnemyCharacterSheet
{

    int attackCooldown = 0;
    int range = 5;
    string projectile = "Arrow";

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

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
    }

    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm)
    {

        if(attackCooldown == 0)
        {

            float distance = Vector3.Distance(transform.position, dum.hero.transform.position);

            //enemy attacks player character if they are in a neighboring tile
            if(range >= distance && LineOfSight.HasLOS(this.gameObject, dum.hero))
            {

                cbm.combatBuffer.Add( new Attack(
                                            this.gameObject, 
                                            dum.hero,
                                            minDamage, 
                                            maxDamage, 
                                            speed,
                                            projectile
                                            ));

                attackCooldown = 3;

            }else //enemy moves towards player character if player is not in range
            {

                List<Vector2Int> pathToPlayer = PathFinder.FindPath(coord, playerCharacter.coord, dum.dungeonCoords); 
                    
                Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), dum.occupiedlist);
            }

        }else
        {

            //randomly walk while reloading ranged attack
            Vector2Int randomDirection = new(coord.x + Direction2D.GetRandomDirection().x, coord.y + Direction2D.GetRandomDirection().y);

            if(dum.dungeonCoords.Contains(randomDirection))
            {

                Move(new Vector3(randomDirection.x, 0.1f, randomDirection.y), dum.occupiedlist);
            }

            attackCooldown -= 1;
        }
    }
}

