using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int position;
    public int width; 
    public int height;

    private static System.Random rand = new System.Random();

    public Room(int x, int y, int width, int height)
    {

        this.position = new Vector2Int(x, y);
        this.width = width;
        this.height = height;
    }

    public HashSet<Vector2Int> GetCoordinates()
    {

        HashSet<Vector2Int> coords = new();
        
        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {

                coords.Add(new Vector2Int(position.x + x, position.y + y));
            }
        }
        
        return coords;
    }

    public Vector2Int GetRandomCoordinate()
    {

        int randomX = rand.Next(position.x, position.x + width);
        int randomY = rand.Next(position.y, position.y + height);

        return new Vector2Int(randomX, randomY);
    }

    public HashSet<Vector2Int> GetPerimeterCoordinates()
    {
        HashSet<Vector2Int> perimeterCoords = new();

        // Top and bottom edges
        for (int x = position.x - 1; x <= position.x + width; x++)
        {
            perimeterCoords.Add(new Vector2Int(x, position.y - 1)); // Top edge
            perimeterCoords.Add(new Vector2Int(x, position.y + height)); // Bottom edge
        }

        // Left and right edges
        for (int y = position.y - 1; y <= position.y + height; y++)
        {
            perimeterCoords.Add(new Vector2Int(position.x - 1, y)); // Left edge
            perimeterCoords.Add(new Vector2Int(position.x + width, y)); // Right edge
        }

        return perimeterCoords;
    }
}

public class BSPNode
{

    public Vector2Int position;
    public int width;
    public int height;
    public BSPNode left;
    public BSPNode right;
    public Room room;
    public bool IsLeaf => left == null && right == null;
    private static System.Random rand = new System.Random();

    public BSPNode(int x, int y, int width, int height)
    {
        
        this.position = new Vector2Int(x, y);
        this.width = width;
        this.height = height;
    }

    public bool Split(int minSize)
    {

        if (!IsLeaf) 
        {
            
            return false;
        }

        bool splitH = rand.NextDouble() > 0.5;

        if (width > height && width / height >= 1.25)
        {

            splitH = false;

        }else if (height > width && height / width >= 1.25)
        {

            splitH = true;
        }

        int max = 0;

        if (splitH)
        {

            max = height - minSize;
        
        }else
        {

            max = width - minSize;
        }

        if (max <= minSize)
        {

            return false;
        }

        int split = rand.Next(minSize, max);

        if (splitH)
        {

            left = new BSPNode(position.x, position.y, width, split);
            right = new BSPNode(position.x, position.y + split, width, height - split);

        }else
        {

            left = new BSPNode(position.x, position.y, split, height);
            right = new BSPNode(position.x + split, position.y, width - split, height);
        }

        return true;
    }

    public void CreateRoom(int buffer = 1)
    {

        int roomWidth = rand.Next(3, width - 2 * buffer);
        int roomHeight = rand.Next(3, height - 2 * buffer);

        int roomX = rand.Next(position.x + buffer, position.x + width - roomWidth - buffer);
        int roomY = rand.Next(position.y + buffer, position.y + height - roomHeight - buffer);

        room = new Room(roomX, roomY, roomWidth, roomHeight);
    }
}

public class BSPGenerator
{

    public static List<Room> Generate(int width, int height, int maxDepth = 5, int minSize = 6)
    {

        List<BSPNode> leaves = new(){ new BSPNode(0, 0, width, height) };

        for (int i = 0; i < maxDepth; i++)
        {

            List<BSPNode> newLeaves = new();

            foreach (BSPNode leaf in leaves)
            {

                if (leaf.Split(minSize))
                {

                    newLeaves.Add(leaf.left);
                    newLeaves.Add(leaf.right);

                }else
                {

                    newLeaves.Add(leaf);
                }
            }

            leaves = newLeaves;
        }

        List<Room> placedRooms = new List<Room>();

        foreach (BSPNode leaf in leaves)
        {

            if (leaf.IsLeaf)
            {

                leaf.CreateRoom();
                placedRooms.Add(leaf.room);
            }
        }

        return placedRooms;
    }
}
