using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGen : MonoBehaviour
{


    public int walkLength;
    Vector2Int startPosition = new Vector2Int(0, 0);
    public GameObject floorTile;
    public GameObject wallTile;

    
    void Start()
    {

        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        var position = startPosition;

        roomGen(position, walkLength, 100, path); 
   
        wallGen(path);

        
    }

    private void roomGen(Vector2Int position, int walkLength, int repeatWalks, HashSet<Vector2Int> path){
        for(int k = 0; k < repeatWalks; k++){

            for(int i = 0; i < walkLength; i++){
                
                if( !path.Contains(position) ){
                    Vector3 spawnPos = new Vector3(position.x, 0, position.y);
                    var newTile = Instantiate(floorTile, spawnPos, floorTile.transform.rotation);
                    newTile.GetComponent<Tile>().setCoord(position);
                    newTile.GetComponent<Renderer>().material.color = Color.black;
                    path.Add(position);
                }

                position = position + Direction2D.getRandomDirection();

            }
        }

    }

    private void wallGen(HashSet<Vector2Int> path){

        HashSet<Vector2Int> wallMap = new HashSet<Vector2Int>();

        foreach(Vector2Int t in path){
            
            foreach(Vector2Int direction in Direction2D.cardinalDirectionsList){

                Vector2Int checkCoord = t + direction;

                if(!path.Contains(checkCoord) && !wallMap.Contains(checkCoord)){
                    
                    Vector3 spawnPos = new Vector3(checkCoord.x, 0, checkCoord.y);
                    var newWall = Instantiate(wallTile, spawnPos, wallTile.transform.rotation);
                    newWall.GetComponent<Renderer>().material.color = Color.gray;
                    wallMap.Add(checkCoord);
                }
            }
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
