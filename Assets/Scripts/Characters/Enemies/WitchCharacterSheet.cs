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

        dropTable = "Witch";
        title = "Witch";

        attackClip = Resources.Load<AudioClip>("Sounds/Mystical");
        teleportClip = Resources.Load<AudioClip>("Sounds/Teleport");
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
            if(teleportCooldown == 0)
            {

                audioSource.PlayOneShot(teleportClip);
                teleportCooldown = 3;

            }else
            {

                Flee(dum, waitTime);
            }

            attackCooldown -= 1;
            teleportCooldown -= 1;
        }
    }
}

