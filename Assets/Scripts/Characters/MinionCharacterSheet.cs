using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionCharacterSheet : EnemyCharacterSheet
{

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
        title = "Skull";

        attackClip = Resources.Load<AudioClip>("Sounds/Skeleton");
    }
    
    public override void AggroBehavior(float waitTime)
    {

        if (!djm.EnemyMap.Keys.Contains(loc.coord))
        {

            Wander(waitTime);
            return;
        }

        List<Vector2Int> neighborPoints = new(PathFinder.GetNeighbors(loc.coord, dum.dungeonCoords));
        neighborPoints = ShuffleNeighbors(neighborPoints);

        Vector2Int targetCoord = loc.coord;
        float minDist = djm.EnemyMap[loc.coord];

        foreach (Vector2Int currCoord in neighborPoints)
        {

            if (!djm.EnemyMap.Keys.Contains(currCoord))
            {

                continue;
            }

            if (djm.EnemyMap[currCoord] == 0)
            {

                AttackEntity(currCoord);
                return;
            }
            else if (djm.EnemyMap[currCoord] <= minDist && !dum.occupiedlist.Contains(currCoord))
            {

                minDist = djm.EnemyMap[currCoord];
                targetCoord = currCoord;
            }
        }

        Move(targetCoord, waitTime);
    }
}