using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using System.Linq;

public class DelaunayTriangulation
{

    public static List<Triangle> GenerateTriangulation(List<Vector2Int> points)
    {

        // Convert Vector2Int points to Triangle.NET's format
        List<Vertex> geometry = new();
        
        foreach (Vector2Int point in points)
        {

            geometry.Add(new Vertex(point.x, point.y));
        }

        // Perform triangulation
        IMesh mesh = new GenericMesher().Triangulate(geometry);

        // Convert the result back to Unity-friendly format
        List<Triangle> triangles = new();

        foreach (var triangle in mesh.Triangles)
        {

            List<Vector2Int> vertices = new()
            {
                
                new((int)triangle.GetVertex(0).X, (int)triangle.GetVertex(0).Y),
                new((int)triangle.GetVertex(1).X, (int)triangle.GetVertex(1).Y),
                new((int)triangle.GetVertex(2).X, (int)triangle.GetVertex(2).Y)
            };

            triangles.Add(new Triangle(vertices));
        }

        return triangles;
    }

    public static HashSet<Edge> GetEdgesFromTriangles(List<Triangle> triangles)
    {

        HashSet<Edge> edges = new();

        foreach (Triangle triangle in triangles)
        {

            edges.Add(new Edge(triangle.Vertices[0], triangle.Vertices[1]));
            edges.Add(new Edge(triangle.Vertices[1], triangle.Vertices[2]));
            edges.Add(new Edge(triangle.Vertices[2], triangle.Vertices[0]));
        }

        return edges;
    }
}

public class Triangle
{

    public List<Vector2Int> Vertices { get; private set; }

    public Triangle(List<Vector2Int> vertices)
    {
        
        Vertices = vertices;
    }
}

public class Edge
{

    public Vector2Int coord1;
    public Vector2Int coord2;

    public Edge(Vector2Int coord1, Vector2Int coord2)
    {

        this.coord1 = coord1;
        this.coord2 = coord2;
    }

    public override bool Equals(object obj)
    {

        if (obj is Edge other)
        {

            return (coord1 == other.coord1 && coord2 == other.coord2) ||
                   (coord1 == other.coord2 && coord2 == other.coord1);
        }

        return false;
    }

    public override int GetHashCode()
    {

        return coord1.GetHashCode() ^ coord2.GetHashCode();
    }
}


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

                //HashSet prevents duplicate edges from being added
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
