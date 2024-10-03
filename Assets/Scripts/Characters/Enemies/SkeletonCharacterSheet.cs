using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCharacterSheet : EnemyCharacterSheet
{

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

        //enemy attacks player character if they are in a neighboring tile
        if(PathFinder.GetNeighbors(playerCharacter.coord, dum.dungeonCoords).Contains(coord))
        {
            
            cbm.AddToCombatBuffer(this.gameObject, dum.hero);

        }else //enemy moves towards player or attack if they are in a neighboring tile                 
        {

            List<Vector2Int> pathToPlayer = PathFinder.FindPath(coord, playerCharacter.coord, dum.dungeonCoords); 

            //If enemy is not neighboring player character...
            if(pathToPlayer.Count > 1)
            {
                
                //...try to move towards player character...
                if(!Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), dum.occupiedlist))
                {                  
                    
                    //...if that spot is occupied then try to path to a tile adjacent to player character 
                    foreach(Vector2Int v in NeighborVals.allDirectionsList)
                    {

                        List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(coord, playerCharacter.coord + v, dum.dungeonCoords);
                        
                        if(pathToPlayerPlusV != null)
                        {

                            if(Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), dum.occupiedlist))
                            {

                                break;
                            }
                        }
                    }
                } 
            } 
        }
    }
}
