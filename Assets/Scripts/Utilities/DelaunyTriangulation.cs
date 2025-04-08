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
        foreach (var point in points)
        {

            geometry.Add(new Vertex(point.x, point.y));
        }

        // Perform triangulation
        var mesh = new GenericMesher().Triangulate(geometry);

        // Convert the result back to Unity-friendly format
        var triangles = new List<Triangle>();
        foreach (var triangle in mesh.Triangles)
        {
            var vertices = new List<Vector2Int>
            {
                new Vector2Int((int)triangle.GetVertex(0).X, (int)triangle.GetVertex(0).Y),
                new Vector2Int((int)triangle.GetVertex(1).X, (int)triangle.GetVertex(1).Y),
                new Vector2Int((int)triangle.GetVertex(2).X, (int)triangle.GetVertex(2).Y)
            };
            triangles.Add(new Triangle(vertices));
        }

        return triangles;
    }

    public static List<Edge> GetEdgesFromTriangles(List<Triangle> triangles)
    {

        List<Edge> edges = new List<Edge>();

        foreach (Triangle triangle in triangles)
        {
            List<Vector2Int> vertices = triangle.Vertices;

            // Add edges for each triangle if they are not already in the list
            Edge edge1 = new Edge(vertices[0], vertices[1]);
            Edge edge2 = new Edge(vertices[1], vertices[2]);
            Edge edge3 = new Edge(vertices[2], vertices[0]);

            if (!edges.Any(e => e.Equals(edge1)))
            {
                edges.Add(edge1);
            }
            if (!edges.Any(e => e.Equals(edge2)))
            {
                edges.Add(edge2);
            }
            if (!edges.Any(e => e.Equals(edge3)))
            {
                edges.Add(edge3);
            }
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
            // Check if the edges connect the same points, regardless of order
            return (coord1 == other.coord1 && coord2 == other.coord2) ||
                   (coord1 == other.coord2 && coord2 == other.coord1);
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Generate a hash code that is independent of the order of the points
        return coord1.GetHashCode() ^ coord2.GetHashCode();
    }
}


public class MinimumSpanningTree
{
    public static List<Edge> CreateMST(List<Edge> edges)
    {
        // Sort edges by weight (distance between points)
        List<Edge> sortedEdges = edges.OrderBy(edge => Vector2Int.Distance(edge.coord1, edge.coord2)).ToList();

        // Initialize the result MST and a union-find structure
        List<Edge> mst = new List<Edge>();
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();

        // Helper function to find the root of a set
        Vector2Int Find(Vector2Int vertex)
        {
            if (!parent.ContainsKey(vertex))
            {
                parent[vertex] = vertex;
            }
            if (parent[vertex] != vertex)
            {
                parent[vertex] = Find(parent[vertex]);
            }
            return parent[vertex];
        }

        // Helper function to union two sets
        void Union(Vector2Int vertex1, Vector2Int vertex2)
        {
            Vector2Int root1 = Find(vertex1);
            Vector2Int root2 = Find(vertex2);
            if (root1 != root2)
            {
                parent[root2] = root1;
            }
        }

        // Process each edge in sorted order
        foreach (Edge edge in sortedEdges)
        {
            Vector2Int root1 = Find(edge.coord1);
            Vector2Int root2 = Find(edge.coord2);

            // If the edge does not form a cycle, add it to the MST
            if (root1 != root2)
            {
                mst.Add(edge);
                Union(root1, root2);
            }
        }

        return mst;
    }

    public static List<Edge> AddExtraEdges(List<Edge> edgesMST, List<Edge> edgesExtra, float addChance)
    {
        // Create a copy of edgesMST to avoid modifying the original list
        List<Edge> resultEdges = new List<Edge>(edgesMST);

        // Iterate through edgesExtra
        foreach (Edge edge in edgesExtra)
        {
            // Check if the edge is not already in edgesMST
            if (!resultEdges.Contains(edge))
            {
                // Add the edge with a probability defined by addChance
                if (Random.value <= addChance)
                {
                    resultEdges.Add(edge);
                }
            }
        }

        return resultEdges;
    }
}
