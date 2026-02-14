using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TextNotificationManager))]
[RequireComponent(typeof(DropLoot))]
public class EnemyCharacterSheet : NpcCharacterSheet
{

    public AudioClip aggroNoise;
    public float aggroRange = 10;

    public override void Awake()
    {

        base.Awake();

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        cbm = managers.GetComponent<CombatManager>();

        djm = GameObject.Find("Map Generator").GetComponent<DijkstraMapManager>();

        aggroNoise = Resources.Load<AudioClip>("Sounds/aggroNoise");
    }

    public override void AggroBehavior()
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }
        
        List<Dictionary<Vector2Int, float>> mapsOfInterest = new(){djm.npcMap, djm.playerMap};
        Vector2Int targetCoord = GetNeighborTileOfMostInterest(loc.coord, mapsOfInterest);
        float tileInterestVal = Mathf.Min(djm.GetPlayerMapValue(targetCoord), djm.GetNpcMapValue(targetCoord));
        
        if(tileInterestVal == 0)
        {

            AttackEntity(targetCoord);

        }else
        {

            movementManager.AddMovement(this, targetCoord);
        }

    }

    public virtual bool OnAggro()
    {

        GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2, "!", Color.red);
        audioSource.PlayOneShot(aggroNoise);
        return true;
    }

    protected virtual bool GetAggroStatus()
    {
        
        if(djm.playerAndNpcMap.ContainsKey(loc.coord))
        {
            
            float aggroVal = djm.playerAndNpcMap[loc.coord];

            if(aggroVal >= aggroRange * 1.5)
            {
                
                return false;
            }
        }

        return true;
    }

    public override void OnDeath()
    {

        GetComponent<DropLoot>().Drop();

        int gainedXP = level * 5;
        entityMgr.playerCharacter.GainXP(gainedXP);
    }
}
