using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterSheet : CharacterSheet
{

    private NameplateManager npm;

    public override void Start()
    {

        base.Start();
        npm = GameObject.Find("CanvasHUD").transform.GetChild(10).GetComponent<NameplateManager>();
    }

    //Custom rules that describe how each enemy reacts when they see the player character
    //Default behavior is running at the player then melee attacking them
    public virtual void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime = 0f)
    {
        
        //enemy attacks player character if they are in a neighboring tile
        if(!cbm.AddMeleeAttack(this.gameObject, dum.hero, minDamage, maxDamage, speed))
        {

            List<Vector2Int> pathToPlayer = PathFinder.FindPath(coord, playerCharacter.coord, dum.dungeonCoords, ignoredPoints: dum.GetOccupiedCoords());
            
            if(pathToPlayer == null)
            {

                pathToPlayer = PathFinder.FindPath(coord, playerCharacter.coord, dum.dungeonCoords);
            }

            Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), dum.occupiedlist, waitTime);
        }
    }

    public virtual void Flee(DungeonManager dum, float waitTime)
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

            Move(new Vector3(fleePath.x, 0.1f, fleePath.y), dum.occupiedlist, waitTime);
        }
    }

    
    public virtual bool OnAggro(DungeonManager dum, CombatManager cbm)
    {

        GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2, "!", Color.red);
        return true;
    }

    public override void Heal(int healValue)
    {

        base.Heal(healValue);
        npm.UpdateHealth();
    }

    public override int TakeDamage(int damage)
    {

        int damageTaken = base.TakeDamage(damage);
        npm.UpdateHealth();

        return damageTaken;
    }
}
