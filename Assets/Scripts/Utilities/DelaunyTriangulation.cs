using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelaunayTriangulation
{

    public static List<Triangle> BowyerWatson(List<Vector2Int> points)
    {

        List<Triangle> triangles = new();

        // Step 1: Create a super triangle that encompasses all points
        Vector2Int minPoint = new(points.Min(p => p.x), points.Min(p => p.y));
        Vector2Int maxPoint = new(points.Max(p => p.x), points.Max(p => p.y));

        int dx = maxPoint.x - minPoint.x;
        int dy = maxPoint.y - minPoint.y;
        int deltaMax = Mathf.Max(dx, dy) * 2;

        Vector2Int p1 = new(minPoint.x - deltaMax, minPoint.y - deltaMax);
        Vector2Int p2 = new(minPoint.x + 3 * deltaMax, minPoint.y - deltaMax);
        Vector2Int p3 = new(minPoint.x - deltaMax, minPoint.y + 3 * deltaMax);

        Triangle superTriangle = new(p1, p2, p3);
        triangles.Add(superTriangle);

        // Step 2: Add each point one at a time
        foreach (Vector2Int point in points)
        {

            List<Triangle> badTriangles = new();

            // Step 2.1: Find all triangles whose circumcircle contains the point
            foreach (Triangle triangle in triangles)
            {

                if (IsPointInCircumcircle(point, triangle))
                {

                    badTriangles.Add(triangle);
                }
            }

            // Step 2.2: Find the boundary of the polygonal hole
            HashSet<Edge> polygon = new();

            foreach (Triangle badTriangle in badTriangles)
            {

                foreach (Edge edge in badTriangle.GetEdges())
                {

                    // If the edge is shared by only one bad triangle, it's part of the polygon
                    if (!polygon.Add(edge))
                    {

                        polygon.Remove(edge);
                    }
                }
            }

            // Step 2.3: Remove bad triangles
            triangles.RemoveAll(t => badTriangles.Contains(t));

            // Step 2.4: Create new triangles from the point to the edges of the polygon
            foreach (Edge edge in polygon)
            {

                triangles.Add(new Triangle(edge.coord1, edge.coord2, point));
            }
        }

        // Step 3: Remove triangles that share vertices with the super triangle
        triangles.RemoveAll(t => t.vert1 == p1 || t.vert1 == p2 || t.vert1 == p3 ||
                                t.vert2 == p1 || t.vert2 == p2 || t.vert2 == p3 ||
                                t.vert3 == p1 || t.vert3 == p2 || t.vert3 == p3);

        return triangles;
    }

    private static bool IsPointInCircumcircle(Vector2Int point, Triangle triangle)
    {

        float ax = triangle.vert1.x - point.x;
        float ay = triangle.vert1.y - point.y;
        float bx = triangle.vert2.x - point.x;
        float by = triangle.vert2.y - point.y;
        float cx = triangle.vert3.x - point.x;
        float cy = triangle.vert3.y - point.y;

        float det = (ax * ax + ay * ay) * (bx * cy - cx * by) -
                    (bx * bx + by * by) * (ax * cy - cx * ay) +
                    (cx * cx + cy * cy) * (ax * by - bx * ay);

        return det > 0;
    }

    public static HashSet<Edge> GetEdgesFromTriangles(List<Triangle> triangles)
    {

        HashSet<Edge> edges = new();

        foreach (Triangle triangle in triangles)
        {

            edges.Add(triangle.edge1);
            edges.Add(triangle.edge1);
            edges.Add(triangle.edge2);
        }

        return edges;
    }
}

public class Triangle
{

    public Vector2Int vert1;
    public Vector2Int vert2;
    public Vector2Int vert3;
    public Edge edge1;
    public Edge edge2;
    public Edge edge3;

    public Triangle(Vector2Int vert1, Vector2Int vert2, Vector2Int vert3)
    {
        
        this.vert1 = vert1;
        this.vert2 = vert2;
        this.vert3 = vert3;

        edge1 = new Edge(vert1, vert2);
        edge2 = new Edge(vert2, vert3);
        edge3 = new Edge(vert3, vert1);
    }

    public HashSet<Edge> GetEdges()
    {

        return new HashSet<Edge>(){

            edge1,
            edge2,
            edge3
        };
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
