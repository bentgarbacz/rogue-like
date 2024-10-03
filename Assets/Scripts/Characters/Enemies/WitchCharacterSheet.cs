using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchCharacterSheet : EnemyCharacterSheet
{

    private int attackCooldown = 0;
    private int teleportCooldown = 0;
    private int range = 5;
    private string projectile = "Magic Missile";
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

        attackClip = Resources.Load<AudioClip>("Sounds/Mystical");
        teleportClip = Resources.Load<AudioClip>("Sounds/Teleport");
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
            if(teleportCooldown == 0)
            {

                audioSource.PlayOneShot(teleportClip);
                teleportCooldown = 10;

            }else
            {

                Flee(dum);
                teleportCooldown -= 1;
            }

            attackCooldown -= 1;
        }
    }

    public void Flee(DungeonManager dum)
    {

        //run away from player
        Vector2Int playerCoord = dum.hero.GetComponent<PlayerCharacterSheet>().coord;
        Vector2Int fleePath  = coord;

        foreach(Vector2Int p in PathFinder.GetNeighbors(coord, dum.dungeonCoords))
        {

            if(PathFinder.CalculateDistance(p, playerCoord) > PathFinder.CalculateDistance(fleePath, playerCoord))
            {

                fleePath = p;
            }
        }

        if(dum.dungeonCoords.Contains(fleePath))
        {

            Move(new Vector3(fleePath.x, 0.1f, fleePath.y), dum.occupiedlist);
        }
    }
}

