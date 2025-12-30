using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcCharacterSheet : CharacterSheet
{

    protected CombatManager cbm;
    protected DijkstraMapManager djm;
    protected NPCMovementManager movementManager;

    public override void Awake()
    {

        base.Awake();

        GameObject managers = GameObject.Find("System Managers");
        entityMgr = managers.GetComponent<EntityManager>();
        cbm = managers.GetComponent<CombatManager>();
        movementManager = managers.GetComponent<NPCMovementManager>();

        djm = GameObject.Find("Map Generator").GetComponent<DijkstraMapManager>();
    }

    public virtual void AggroBehavior()
    {

        return;
    }

    public virtual void Wander()
    {

        List<Vector2Int> neighbors = new(PathFinder.GetNeighbors(loc.coord, tileMgr.levelCoords));

        if (neighbors.Count == 0)
        {

            return;
        }

        Vector2Int chosenCoord = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
        movementManager.AddMovement(this, chosenCoord);
        //Move(chosenCoord, waitTime);

        //Vector2Int randomDirection = new(loc.coord.x + Direction2D.GetRandomDirection().x, loc.coord.y + Direction2D.GetRandomDirection().y);

        //if(tileMgr.levelCoords.Contains(randomDirection))
        //{

        //    Move(randomDirection, waitTime);
        //}
    }

    public virtual void Flee(Dictionary<Vector2Int, float> fleeMap)
    {

        Vector2Int fleePath = loc.coord;
        float fleeVal = float.MinValue;

        foreach (Vector2Int currCoord in PathFinder.GetNeighbors(loc.coord, tileMgr.levelCoords))
        {

            if(!fleeMap.ContainsKey(currCoord))
            {
                
                continue;
            }

            if(fleeMap[currCoord] > fleeVal && !tileMgr.occupiedlist.Contains(currCoord))
            {
                
                fleeVal = fleeMap[currCoord];
                fleePath = currCoord;
            }        
        }

        if (tileMgr.levelCoords.Contains(fleePath))
        {
            
            movementManager.AddMovement(this, fleePath);
        }
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

    protected virtual Vector2Int GetNeighborTileOfMostInterest(Vector2Int startCoord, List<Dictionary<Vector2Int, float>> mapsOfInterest)
    {

        List<Vector2Int> neighborPoints = new(PathFinder.GetNeighbors(startCoord, tileMgr.levelCoords));
        neighborPoints = ShuffleNeighbors(neighborPoints);

        List<Vector2Int> cardinalNeighbors = GetCardinalNeighbors(neighborPoints);

        Vector2Int targetCoord = startCoord;
        float minDist = float.MaxValue;

        foreach (Vector2Int currCoord in neighborPoints)
        {

            float currMinVal = float.MaxValue;

            foreach (Dictionary<Vector2Int, float> currMap in mapsOfInterest)
            {

                if (currMap.TryGetValue(currCoord, out float value))
                    currMinVal = Mathf.Min(currMinVal, value);
            }

            if (currMinVal == 0)
            {

                return currCoord;

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

        return targetCoord;
    }

    protected virtual Vector2Int GetRangedTarget(Vector2Int startCoord, List<Dictionary<Vector2Int, float>> mapsOfInterest, int recursiveDepth = 0)
    {

        if(recursiveDepth >= 30)
        {
            
            return new Vector2Int(int.MaxValue, int.MaxValue);
        }

        float currVal = float.MaxValue;

        foreach (Dictionary<Vector2Int, float> currMap in mapsOfInterest)
        {

            if (currMap.TryGetValue(startCoord, out float value))
                    currVal = Mathf.Min(currVal, value);
        }

        if(currVal == 0)
        {
            
            return startCoord;

        }
        else
        {
            
            Vector2Int nextCoord = GetNeighborTileOfMostInterest(startCoord, mapsOfInterest);
            return GetRangedTarget(nextCoord, mapsOfInterest, recursiveDepth + 1);
        }
    }
}
