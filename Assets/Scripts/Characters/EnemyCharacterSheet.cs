using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TextNotificationManager))]
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

    public override void AggroBehavior(float waitTime = 0f)
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }

        List<Vector2Int> neighborPoints = new(PathFinder.GetNeighbors(loc.coord, tileMgr.levelCoords));
        neighborPoints = ShuffleNeighbors(neighborPoints);

        List<Vector2Int> cardinalNeighbors = GetCardinalNeighbors(neighborPoints);

        Vector2Int targetCoord = loc.coord;
        float minDist = float.MaxValue;

        foreach (Vector2Int currCoord in neighborPoints)
        {

            float currPlayerVal = djm.GetPlayerMapValue(currCoord);
            float currNpcVal = djm.GetNpcMapValue(currCoord);

            if (currPlayerVal == float.MaxValue && currNpcVal == float.MaxValue)
            {

                continue;
            }

            float currMinVal = Mathf.Min(currPlayerVal, currNpcVal);

            if (currMinVal == 0)
            {

                AttackEntity(currCoord);
                return;
            }
            else if (currMinVal <= minDist && !tileMgr.occupiedlist.Contains(currCoord))
            {

                if (cardinalNeighbors.Contains(targetCoord) && !cardinalNeighbors.Contains(currCoord) && currMinVal == minDist)
                {

                    continue;
                }

                minDist = currMinVal;
                targetCoord = currCoord;
            }
        }

        Move(targetCoord, waitTime);
    }

    public virtual bool OnAggro()
    {

        GetComponent<TextNotificationManager>().CreateNotificationOrder(transform.position, 2, "!", Color.red);
        audioSource.PlayOneShot(aggroNoise);
        return true;
    }

    protected virtual bool GetAggroStatus()
    {

        if (djm.playerMap.ContainsKey(loc.coord) || djm.npcMap.ContainsKey(loc.coord))
        {

            return true;
        }

        return false;
    }
}
