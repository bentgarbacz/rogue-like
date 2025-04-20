using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinimumSpanningTree
{

    private static Vector2Int Find(Vector2Int vertex, Dictionary<Vector2Int, Vector2Int> parent)
    {

        if (!parent.ContainsKey(vertex))
        {

            parent[vertex] = vertex;        
        }
        
        if (parent[vertex] != vertex)
        {

            parent[vertex] = Find(parent[vertex], parent);
        }

        return parent[vertex];
    }
    
    private static void Union(Vector2Int vertex1, Vector2Int vertex2, Dictionary<Vector2Int, Vector2Int> parent)
    {

        Vector2Int root1 = Find(vertex1, parent);
        Vector2Int root2 = Find(vertex2, parent);

        if (root1 != root2)
        {

            parent[root2] = root1;
        }
    }

    public static HashSet<Edge> CreateMST(HashSet<Edge> edges)
    {
        
        // Sort edges by weight (distance between points)
        List<Edge> sortedEdges = edges.OrderBy(edge => Vector2Int.Distance(edge.coord1, edge.coord2)).ToList();

        // Initialize the result MST and a union-find structure
        HashSet<Edge> mst = new();
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();

        // Process each edge in sorted order
        foreach (Edge edge in sortedEdges)
        {

            Vector2Int root1 = Find(edge.coord1, parent);
            Vector2Int root2 = Find(edge.coord2, parent);

            // If the edge does not form a cycle, add it to the MST
            if (root1 != root2)
            {

                mst.Add(edge);
                Union(root1, root2, parent);
            }
        }

        return mst;
    }

    public static HashSet<Edge> AddExtraEdges(HashSet<Edge> edgesMST, HashSet<Edge> edgesExtra, float addChance)
    {

        HashSet<Edge> resultEdges = new(edgesMST);

        foreach (Edge edge in edgesExtra)
        {

            if (Random.value <= addChance)
            {

                resultEdges.Add(edge);
            }
        }

        return resultEdges;
    }

    public static HashSet<Edge> CreateMSTExtraEdges(HashSet<Edge> edges, float addChance)
    {

        return AddExtraEdges(CreateMST(edges), edges, addChance);
    }
}
