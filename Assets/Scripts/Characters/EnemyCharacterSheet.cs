using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCharacterSheet : CharacterSheet
{

    public AudioClip aggroNoise;
    public float aggroRange = 10;
    protected CombatManager cbm;
    protected DijkstraMapManager djm;

    public override void Awake()
    {

        base.Awake();

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        cbm = managers.GetComponent<CombatManager>();

        djm = GameObject.Find("Map Generator").GetComponent<DijkstraMapManager>();

        aggroNoise = Resources.Load<AudioClip>("Sounds/aggroNoise");
    }

    public virtual void AggroBehavior(float waitTime = 0f)
    {

        if (!GetAggroStatus())
        {

            entityMgr.aggroEnemies.Remove(this.gameObject);
            return;
        }

        List<Vector2Int> neighborPoints = new(PathFinder.GetNeighbors(loc.coord, tileMgr.dungeonCoords));
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

    public virtual void Wander(float waitTime)
    {

        Vector2Int randomDirection = new(loc.coord.x + Direction2D.GetRandomDirection().x, loc.coord.y + Direction2D.GetRandomDirection().y);

        if(tileMgr.dungeonCoords.Contains(randomDirection))
        {

            Move(randomDirection, waitTime);
        }
    }

    public virtual void Flee(float waitTime)
    {

        //run away from player
        Vector2Int playerCoord = entityMgr.hero.GetComponent<PlayerCharacterSheet>().loc.coord;
        Vector2Int fleePath = loc.coord;

        foreach (Vector2Int p in PathFinder.GetNeighbors(loc.coord, tileMgr.dungeonCoords))
        {

            if (PathFinder.CalculateDistance(p, playerCoord) > PathFinder.CalculateDistance(fleePath, playerCoord))
            {

                fleePath = p;
            }
        }

        if (tileMgr.dungeonCoords.Contains(fleePath))
        {

            Move(fleePath, waitTime);
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

        if (djm.playerMap.ContainsKey(loc.coord) || djm.npcMap.ContainsKey(loc.coord))
        {

            return true;
        }

        return false;
    }

    protected List<Vector2Int> ShuffleNeighbors(List<Vector2Int> neighborPoints)
    {

        List<Vector2Int> shuffledNeighbors = new(neighborPoints);

        for (int i = shuffledNeighbors.Count - 1; i > 0; i--)
        {

            int j = Random.Range(0, i + 1);
            Vector2Int temp = shuffledNeighbors[i];
            shuffledNeighbors[i] = shuffledNeighbors[j];
            shuffledNeighbors[j] = temp;
        }

        return shuffledNeighbors;
    }

    protected List<Vector2Int> GetCardinalNeighbors(List<Vector2Int> neighborPoints)
    {

        List<Vector2Int> cardinalNeighbors = new();

        foreach (Vector2Int point in neighborPoints)
        {

            Vector2Int offset = point - loc.coord;

            if (Direction2D.cardinalDirectionsList.Contains(offset))
            {

                cardinalNeighbors.Add(point);
            }

        }

        return cardinalNeighbors;
    }

    protected virtual bool AttackEntity(Vector2Int coord)
    {

        GameObject targetEntity = null;

        foreach (GameObject entity in tileMgr.GetTile(coord).entitiesOnTile)
        {

            if (entity != null && entity.GetComponent<CharacterSheet>() != null)
            {

                targetEntity = entity;
            }
        }

        if (targetEntity != null)
        {

            return cbm.AddMeleeAttack(this.gameObject, targetEntity, minDamage, maxDamage, speed);
        }

        return false;
    }
}
