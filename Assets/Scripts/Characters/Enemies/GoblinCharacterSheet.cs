using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinCharacterSheet : EnemyCharacterSheet
{

    public override void Start()
    {
        
        base.Start();
        maxHealth = 10;
        health = maxHealth;
        accuracy = 66;
        minDamage = 1;
        maxDamage = 4;
        level = 4;
        speed = 8;
        evasion = 50;

        dropTable = "Goblin";
        title = "Goblin";

        attackClip = Resources.Load<AudioClip>("Sounds/Frog");
    }

    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime = 0f)
    {

        //enemy attacks player character if they are in a neighboring tile
        if(!cbm.AddMeleeAttack(this.gameObject, dum.hero, minDamage, maxDamage, speed))
        {

            List<Vector2Int> pathToPlayer = PathFinder.FindPath(coord, playerCharacter.coord, dum.dungeonCoords); 

            //If enemy is not neighboring player character...
            if(pathToPlayer.Count > 1)
            {
                
                //...try to move towards player character...
                if(!Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), dum.occupiedlist, waitTime))
                {                  
                    
                    //...if that spot is occupied then try to path to a tile adjacent to player character 
                    foreach(Vector2Int v in NeighborVals.allDirectionsList)
                    {

                        List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(coord, playerCharacter.coord + v, dum.dungeonCoords);
                        
                        if(pathToPlayerPlusV != null)
                        {

                            if(Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), dum.occupiedlist, waitTime))
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