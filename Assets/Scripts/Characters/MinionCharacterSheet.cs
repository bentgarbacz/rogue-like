using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionCharacterSheet : NpcCharacterSheet
{

    private bool isSummoningSick = true;

    public override void Awake()
    {

        base.Awake();
    }
    
    public override void AggroBehavior()
    {

        if (isSummoningSick)
        {

            isSummoningSick = false;
            return;
        }

        if (!djm.enemyMap.Keys.Contains(loc.coord))
        {

            Wander();
            return;
        }

        List<Vector2Int> neighborPoints = new(PathFinder.GetNeighbors(loc.coord, tileMgr.levelCoords));
        neighborPoints = ShuffleNeighbors(neighborPoints);

        Vector2Int targetCoord = loc.coord;
        float minDist = djm.enemyMap[loc.coord];

        foreach (Vector2Int currCoord in neighborPoints)
        {

            if (!djm.enemyMap.Keys.Contains(currCoord))
            {

                continue;
            }

            if (djm.enemyMap[currCoord] == 0)
            {

                AttackEntity(currCoord);
                return;
            }
            else if (djm.enemyMap[currCoord] <= minDist && !tileMgr.occupiedlist.Contains(currCoord))
            {

                minDist = djm.enemyMap[currCoord];
                targetCoord = currCoord;
            }
        }

        movementManager.AddMovement(this, targetCoord);
    }
}