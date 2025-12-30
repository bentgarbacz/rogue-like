using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCharacterSheet : EnemyCharacterSheet
{
   
    private bool falling = false;
    private float fallSpeed = 30f;
    private Vector2Int landingCoord;
    private bool landingFound = false;
    private ObjectVisibility objectVisibility;
    private TurnSequencer turnSequencer;

    public override void Awake()
    {

        base.Awake();
        maxHealth = 10;
        accuracy = 66;
        minDamage = 1;
        maxDamage = 4;
        level = 4;
        speed = 8;
        evasion = 50;

        characterHealth.InitHealth(maxHealth);

        dropTable = "Slime";
        title = "Slime";

        attackClip = Resources.Load<AudioClip>("Sounds/Slime");

        objectVisibility = GetComponent<ObjectVisibility>();
        objectVisibility.isActive = false;

        turnSequencer = managers.GetComponent<TurnSequencer>();
    }

    void Update()
    {
        
        if(falling == true)
        {

            transform.position = Vector3.MoveTowards(
                                                        transform.position, 
                                                        entityMgr.playerCharacter.transform.position, 
                                                        fallSpeed * Time.deltaTime
                                                    );

            if(transform.position == entityMgr.playerCharacter.transform.position)
            {

                falling = false;
                audioSource.PlayOneShot(attackClip);
                cbm.ExecuteAttack(this.gameObject, entityMgr.hero, minDamage, maxDamage, speed);
                tileMgr.occupiedlist.Remove(landingCoord);
                Move(landingCoord);
                turnSequencer.HaltGameplay(false);
            }
        }
    }

    public override bool OnAggro()
    {

        if(objectVisibility.isActive)
        {
        
            return base.OnAggro();
        }

        HashSet<Vector2Int> landingCoords = PathFinder.GetNeighbors(entityMgr.playerCharacter.loc.coord, tileMgr.levelCoords);

        foreach(Vector2Int possibleCoord in landingCoords)
        {
            
            if(tileMgr.occupiedlist.Add(possibleCoord))
            {

                landingFound = true;
                landingCoord = possibleCoord;
                break;
            }
        }

        if(landingFound)
        {

            objectVisibility.isActive = true;

            transform.position = entityMgr.hero.transform.position + new Vector3(0, 10f, 0);
            objectVisibility.SetVisibility(true);
            falling = true;
            turnSequencer.HaltGameplay();

            return true;
        }

        return false;
    }

    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}