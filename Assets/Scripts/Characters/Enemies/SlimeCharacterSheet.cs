using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCharacterSheet : EnemyCharacterSheet
{
   
    private bool hasDropped = false;
    private bool falling = false;
    private float fallSpeed = 30f;
    private Vector2Int landingCoord;
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
                dum.occupiedlist.Remove(landingCoord);
                Move(landingCoord, dum.occupiedlist);
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
                
                if(dum.occupiedlist.Add(possibleCoord))
                {

                    landingFound = true;
                    landingCoord = possibleCoord;
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

        base.AggroBehavior(playerCharacter, dum, cbm, waitTime);
    }
}