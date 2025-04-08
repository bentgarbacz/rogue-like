using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2Int Position;
    public int Width, Height;

    private static System.Random rand = new System.Random();

    public Room(int x, int y, int width, int height)
    {

        Position = new Vector2Int(x, y);
        Width = width;
        Height = height;
    }

    public HashSet<Vector2Int> GetCoordinates()
    {

        HashSet<Vector2Int> coords = new();
        
        for (int x = 0; x < Width; x++)
        {

            for (int y = 0; y < Height; y++)
            {

                coords.Add(new Vector2Int(Position.x + x, Position.y + y));
            }
        }
        
        return coords;
    }

    public Vector2Int GetRandomCoordinate()
    {
        int randomX = rand.Next(Position.x, Position.x + Width);
        int randomY = rand.Next(Position.y, Position.y + Height);
        return new Vector2Int(randomX, randomY);
    }
}

public class BSPNode
{

    public Vector2Int Position;
    public int Width, Height;
    public BSPNode Left, Right;
    public Room Room;

    private static System.Random rand = new System.Random();

    public BSPNode(int x, int y, int width, int height)
    {
        
        Position = new Vector2Int(x, y);
        Width = width;
        Height = height;
    }

    public bool IsLeaf => Left == null && Right == null;

    public bool Split(int minSize)
    {
        if (!IsLeaf) return false;

        bool splitH = rand.NextDouble() > 0.5;
        if (Width > Height && Width / Height >= 1.25)
            splitH = false;
        else if (Height > Width && Height / Width >= 1.25)
            splitH = true;

        int max = (splitH ? Height : Width) - minSize;
        if (max <= minSize)
            return false;

        int split = rand.Next(minSize, max);

        if (splitH)
        {
            Left = new BSPNode(Position.x, Position.y, Width, split);
            Right = new BSPNode(Position.x, Position.y + split, Width, Height - split);
        }
        else
        {
            Left = new BSPNode(Position.x, Position.y, split, Height);
            Right = new BSPNode(Position.x + split, Position.y, Width - split, Height);
        }

        return true;
    }

    public void CreateRoom(int buffer = 1)
    {
        int roomWidth = rand.Next(3, Width - 2 * buffer);
        int roomHeight = rand.Next(3, Height - 2 * buffer);

        int roomX = rand.Next(Position.x + buffer, Position.x + Width - roomWidth - buffer);
        int roomY = rand.Next(Position.y + buffer, Position.y + Height - roomHeight - buffer);

        Room = new Room(roomX, roomY, roomWidth, roomHeight);
    }
}

public class BSPGenerator
{
    public static List<Room> Generate(int width, int height, int maxDepth = 5, int minSize = 6)
    {
        var root = new BSPNode(0, 0, width, height);
        List<BSPNode> leaves = new List<BSPNode> { root };

        for (int i = 0; i < maxDepth; i++)
        {
            List<BSPNode> newLeaves = new List<BSPNode>();
            foreach (var leaf in leaves)
            {
                if (leaf.Split(minSize))
                {
                    newLeaves.Add(leaf.Left);
                    newLeaves.Add(leaf.Right);
                }
                else
                {
                    newLeaves.Add(leaf);
                }
            }
            leaves = newLeaves;
        }

        List<Room> placedRooms = new List<Room>();

        foreach (var leaf in leaves)
        {
            if (leaf.IsLeaf)
            {
                leaf.CreateRoom();
                placedRooms.Add(leaf.Room);
            }
        }

        return placedRooms;
    }
}
