using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGen : MonoBehaviour
{


    public int walkLength;
    Vector2Int startPosition = new Vector2Int(0, 0);
    public GameObject floorTile;

    
    void Start()
    {

        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        var position = startPosition;

        for(int i = 0; i < walkLength; i++){
            
            if( !path.Contains(position) ){
                Vector3 spawnPos = new Vector3(position.x, 0, position.y);
                Instantiate(floorTile, spawnPos, floorTile.transform.rotation);
                path.Add(position);
            }
            position = position + Direction2D.getRandomDirection();

        }
        
    }

}

public static class Direction2D{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>{
        new Vector2Int(0, 1), //up
        new Vector2Int(1, 0), //right
        new Vector2Int(0, -1), //down
        new Vector2Int(-1, 0) //left
    };

    public static Vector2Int getRandomDirection(){
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
