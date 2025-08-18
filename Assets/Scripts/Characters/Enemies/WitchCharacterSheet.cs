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
            if(teleportCooldown == 0)
            {

                audioSource.PlayOneShot(teleportClip);
                teleportCooldown = 3;

            }else
            {

                Flee(waitTime);
            }

            attackCooldown -= 1;
            teleportCooldown -= 1;
        }
    }
}

