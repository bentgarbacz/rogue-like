using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new()
    {

        new Vector2Int(0, 1), //North
        new Vector2Int(1, 0), //East
        new Vector2Int(0, -1), //South
        new Vector2Int(-1, 0) //West
    };

    public static List<Vector2Int> intercardinalDirectionsList = new()
    {

        new Vector2Int(1, 1), //North East
        new Vector2Int(1, -1), //South East
        new Vector2Int(-1, -1), //South West
        new Vector2Int(-1, 1) //North West
    };

    public static List<Vector2Int> DirectionsList()
    {

        return intercardinalDirectionsList.Concat(cardinalDirectionsList).ToList();
    }

    public static Vector2Int GetRandomDirection()
    {

        return cardinalDirectionsList[UnityEngine.Random.Range(0, cardinalDirectionsList.Count)];
    }
}