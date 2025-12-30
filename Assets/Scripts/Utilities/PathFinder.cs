using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public static class PathFinder
{

    //returns a path from start to destination on a grid
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int destination, HashSet<Vector2Int> grid, bool useIntercardinal = true, Dictionary<Vector2Int, float> costDict = null, HashSet<Vector2Int> ignoredPoints = null)
    {
        
        //List<Vector2Int> path = new List<Vector2Int>();

        HashSet<Vector2Int> closedSet = new();
        HashSet<Vector2Int> openSet = new();

        if(ignoredPoints != null)
        {

            //If ignoredPoints contains start or destination, we dont want those points in the closed set, so we remove them
            closedSet.UnionWith(ignoredPoints);
            closedSet.Remove(start);
            closedSet.Remove(destination);
        }

        Dictionary<Vector2Int, AStarNode> nodeMap = new();

        float hcostModStart = 0f;

        if(costDict != null)
        {

            hcostModStart = costDict[start];
        }

        AStarNode startNode = new(start, null, 0, CalculateDistance(start, destination) + hcostModStart);
        openSet.Add(start);
        nodeMap[start] = startNode;

        while(openSet.Count > 0)
        {

            Vector2Int currentPos = GetLowestFCostNode(openSet, nodeMap);
            AStarNode currentNode = nodeMap[currentPos];

            if(currentPos.Equals(destination))
            {

                return ReconstructPath(currentNode);
            }

            openSet.Remove(currentPos);
            closedSet.Add(currentPos);

            foreach(Vector2Int neighborPos in GetNeighbors(currentPos, grid, useIntercardinal))
            {

                if(closedSet.Contains(neighborPos))
                {
                
                    continue;
                }

                float tentativeGCost = currentNode.gCost + CalculateDistance(currentPos, neighborPos);

                if(!openSet.Contains(neighborPos) || tentativeGCost < nodeMap[neighborPos].gCost)
                {

                    float hcostMod = 0f;

                    if(costDict != null)
                    {

                        hcostMod = costDict[neighborPos];
                    }

                    AStarNode neighborNode = new(neighborPos, currentNode, tentativeGCost, CalculateDistance(neighborPos, destination) + hcostMod);
                    nodeMap[neighborPos] = neighborNode;

                    if(!openSet.Contains(neighborPos))
                    {

                        openSet.Add(neighborPos);
                    }
                }
            }
        }

        return null;
    }

    public static HashSet<Vector2Int> GetNeighbors(Vector2Int point, HashSet<Vector2Int> grid, bool useIntercardinal = true)
    {

        HashSet<Vector2Int> neighbors = new();

        List<Vector2Int> directions = NeighborVals.allDirectionsList;

        if(!useIntercardinal)
        {

            directions = NeighborVals.cardinalList;
        }
        
        foreach(Vector2Int d in directions)
        {

            Vector2Int checkPoint = new(point.x + d.x, point.y + d.y);

            if(grid.Contains(checkPoint))
            {

                if(d.x != 0 && d.y != 0)
                {
                    Vector2Int pinchPointOne = new(point.x, point.y + d.y);
                    Vector2Int pinchPointTwo = new(point.x + d.x, point.y);

                    if(grid.Contains(pinchPointOne) || grid.Contains(pinchPointTwo))
                    {

                        neighbors.Add(checkPoint);
                    }

                }else{

                    neighbors.Add(checkPoint);
                }
            }
        }

        return neighbors;
    }

    private static List<Vector2Int> ReconstructPath(AStarNode node)
    {

        List<Vector2Int> path = new();

        while(node != null)
        {

            path.Add(node.position);
            node = node.parent;
        }

        path.Reverse();
        return path;
    }

    private static Vector2Int GetLowestFCostNode(HashSet<Vector2Int> openSet, Dictionary<Vector2Int, AStarNode> nodeMap)
    {

        Vector2Int lowestFCostCoord = default;
        float minFCost = float.MaxValue;

        foreach(Vector2Int coord in openSet)
        {

            float currentFCost = nodeMap[coord].fCost;

            if(currentFCost < minFCost)
            {

                minFCost = currentFCost;
                lowestFCostCoord = coord; 
            }
        }

        return lowestFCostCoord;
    }

    public static void PrecachePath(Vector2Int start, Vector2Int destination, HashSet<Vector2Int> grid, Dictionary<string, List<Vector2Int>> PrecachedPaths)
    {

        List<Vector2Int> path = FindPath(start, destination, grid);

        if(path != null)
        {

            PathKey pk = new(start, destination);
            PrecachedPaths.Add(pk.key, path);
        }
    }

    public static float CalculateDistance(Vector2Int a, Vector2Int b)
    {

        float x1 = a.x;
        float x2 = b.x;
        float y1 = a.y;
        float y2 = b.y;

        return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    public static HashSet<Vector2Int> GenerateBlankGrid(int width, int height)
    {
        
        HashSet<Vector2Int> grid = new();

        for (int x = 0; x <= width; x++)
        {

            for (int y = 0; y <= height; y++)
            {

                grid.Add(new Vector2Int(x, y));
            }
        }

        return grid;
    }
}

public static class NeighborVals{

    public static List<Vector2Int> allDirectionsList = new()
    {

        new Vector2Int(0, 1), //up
        new Vector2Int(1, 0), //right
        new Vector2Int(0, -1), //down
        new Vector2Int(-1, 0), //left
        new Vector2Int(1, 1), //up right
        new Vector2Int(-1, 1), //up left
        new Vector2Int(1, -1), //down right
        new Vector2Int(-1, -1) //down left
    };

    public static List<Vector2Int> cardinalList = new()
    {

        new Vector2Int(0, 1), //up
        new Vector2Int(1, 0), //right
        new Vector2Int(0, -1), //down
        new Vector2Int(-1, 0), //left
    };
}

public class AStarNode
{

    public Vector2Int position;
    public AStarNode parent;
    public float gCost;
    public float hCost;
    public float fCost;

    public AStarNode (Vector2Int position, AStarNode parent, float gCost, float hCost)
    {

        this.position = position;
        this.parent = parent;
        this.gCost = gCost;
        this.hCost = hCost;
        this.fCost = gCost + hCost;
    }
}

public class PathKey
{

    public Vector2Int start;
    public Vector2Int destination;
    public string key;

    public PathKey(Vector2Int start, Vector2Int destination)
    {

        this.start = start;
        this.destination = destination;
        key = "sx" +
            start.x.ToString() +
            "sy" +
            start.y.ToString() +
            "dx" +
            destination.x.ToString() +
            "dy" +
            destination.y.ToString(); 
    }
}