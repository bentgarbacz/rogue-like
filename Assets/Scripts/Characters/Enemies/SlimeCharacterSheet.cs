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
        stats.maxHealth = 10;
        stats.accuracy = 66;
        stats.minDamage = 1;
        stats.maxDamage = 4;
        level = 4;
        stats.speed = 8;
        stats.evasion = 50;

        dropTable = DropTableType.Slime;
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
                Attack attack = new(this.gameObject, entityMgr.hero, stats.minDamage, stats.maxDamage, stats.speed);
                combatSeq.ExecuteAttack(attack);
                tileMgr.occupiedlist.Remove(landingCoord);
                Move(landingCoord);
                lockMgr.ReleaseTurnLock();
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
            lockMgr.AcquireTurnLock();

            return true;
        }

        return false;
    }

    public override void AggroBehavior()
    {

        base.AggroBehavior();
    }
}