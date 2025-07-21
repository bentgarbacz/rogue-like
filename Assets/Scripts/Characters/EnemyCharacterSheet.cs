using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterSheet : CharacterSheet
{

    public AudioClip aggroNoise;
    public float aggroRange = 10;

    public override void Start()
    {

        base.Start();

        aggroNoise = Resources.Load<AudioClip>("Sounds/aggroNoise");
    }

    //Custom rules that describe how each enemy reacts when they see the player character
    //Default behavior is running at the player then melee attacking them
    public virtual void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime = 0f)
    {

        //enemy attacks player character if they are in a neighboring tile
        if (!cbm.AddMeleeAttack(this.gameObject, dum.hero, minDamage, maxDamage, speed))
        {

            List<Vector2Int> pathToPlayer = PathFinder.FindPath(loc.coord, playerCharacter.loc.coord, dum.dungeonCoords, ignoredPoints: dum.occupiedlist);

            if (pathToPlayer == null)
            {

                pathToPlayer = PathFinder.FindPath(loc.coord, playerCharacter.loc.coord, dum.dungeonCoords);
            }

            Move(pathToPlayer[1], dum.occupiedlist, waitTime);
        }
    }

    public virtual void Flee(DungeonManager dum, float waitTime)
    {

        //run away from player
        Vector2Int playerCoord = dum.hero.GetComponent<PlayerCharacterSheet>().loc.coord;
        Vector2Int fleePath = loc.coord;

        foreach (Vector2Int p in PathFinder.GetNeighbors(loc.coord, dum.dungeonCoords))
        {

            if (PathFinder.CalculateDistance(p, playerCoord) > PathFinder.CalculateDistance(fleePath, playerCoord))
            {

                fleePath = p;
            }
        }

        if (dum.dungeonCoords.Contains(fleePath))
        {

            Move(fleePath, dum.occupiedlist, waitTime);
        }
    }


    public virtual bool OnAggro(DungeonManager dum, CombatManager cbm)
    {

        GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2, "!", Color.red);
        audioSource.PlayOneShot(aggroNoise);
        return true;
    }
}
