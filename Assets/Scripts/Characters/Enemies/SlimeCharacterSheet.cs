using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCharacterSheet : EnemyCharacterSheet
{
   
    private bool hasDropped = false;
    private bool falling = false;
    private float fallSpeed = 30f;
    private Vector3 landingPos;
    private bool landingFound = false;
    private DungeonManager dum;
    private CombatManager cbm;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

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

        dropTable = "Slime";
        title = "Slime";

        attackClip = Resources.Load<AudioClip>("Sounds/Slime");

        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    void Update()
    {
        
        if(falling == true)
        {

            transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        dum.playerCharacter.pos, 
                                                        fallSpeed * Time.deltaTime
                                                    );

            if(transform.position == dum.playerCharacter.pos)
            {

                falling = false;
                audioSource.PlayOneShot(attackClip);
                cbm.ExecuteAttack(this.gameObject, dum.hero, minDamage, maxDamage, speed);
                Move(landingPos, dum.occupiedlist);
                dum.HaltGameplay(false);
            }
        }
    }

    public override bool OnAggro(DungeonManager dum, CombatManager cbm)
    {

        HashSet<Vector2Int> landingCoords = PathFinder.GetNeighbors(dum.playerCharacter.coord, dum.dungeonCoords);

        if(!hasDropped)
        {

            foreach(Vector2Int possibleCoord in landingCoords)
            {
                Vector3 possiblePos = new(possibleCoord.x, 0.1f, possibleCoord.y);

                if(!dum.occupiedlist.Contains(possiblePos))
                {

                    landingFound = true;
                    landingPos = possiblePos;
                    break;
                }
            }

            if(landingFound)
            {

                this.dum = dum;
                this.cbm = cbm;
                hasDropped = true;

                transform.position = dum.hero.transform.position + new Vector3(0, 10f, 0);
                meshRenderer.enabled = true;
                boxCollider.enabled = true;
                falling = true;
                dum.HaltGameplay();

                return true;
            }
        }

        return false;
    }

    public override void AggroBehavior(PlayerCharacterSheet playerCharacter, DungeonManager dum, CombatManager cbm, float waitTime)
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