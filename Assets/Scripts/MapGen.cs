using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGen : MonoBehaviour
{


    public int walkLength;
    Vector2Int startPosition = new Vector2Int(0, 0);
    public GameObject floorTile;
    public GameObject wallTile;
    public GameObject enterance;
    public GameObject exit;
    public GameObject hero;
    public GameObject chest;
    public GameObject mainCamera;
    
    void Start()
    {

        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        var position = startPosition;

        roomGen(position, walkLength, 100, path); 
   
        wallGen(path);

        
    }

    private void roomGen(Vector2Int position, int walkLength, int repeatWalks, HashSet<Vector2Int> path){

        bool exitSpawned = false;

        for(int i = 0; i < repeatWalks; i++){

            for(int j = 0; j < walkLength; j++){
                
                if( !path.Contains(position) ){

                    Vector3 spawnPos = new Vector3(position.x, 0, position.y);
                    int spawnRNG = Random.Range(0, 1000);

                    if(i == 0 && j == 0){

                        var newTile = Instantiate(enterance, spawnPos, enterance.transform.rotation);
                        newTile.GetComponent<Tile>().setCoord(position);

                    }else if(i == repeatWalks - 1 && j > walkLength / 2 && exitSpawned == false){

                        exitSpawned = true;
                        spawnPos.y -= 1;
                        var newTile = Instantiate(exit, spawnPos, exit.transform.rotation);
                        newTile.GetComponent<Tile>().setCoord(position);
                    
                    }else{
                        var newTile = Instantiate(floorTile, spawnPos, floorTile.transform.rotation);
                        newTile.GetComponent<Tile>().setCoord(position);
                    }

                    spawnPos.y += 0.5f;
                    if(i == 0 && j == 1){
                        //hero.transform.position = spawnPos;
                        Instantiate(hero, spawnPos, hero.transform.rotation);
                        
                        mainCamera.GetComponent<playerCamera>().setFocalPoint(hero);
                    }else if(spawnRNG >= 0 && spawnRNG <= 5){
                        Instantiate(chest, spawnPos, chest.transform.rotation);
                    }
                    
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
