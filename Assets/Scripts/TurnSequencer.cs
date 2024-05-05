using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public GameObject hero;
    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<GameObject> enemies = new HashSet<GameObject>();
    public HashSet<GameObject> aggroEnemies = new HashSet<GameObject>();
    public HashSet<Vector3> occupiedlist;
    public List<Vector2Int> bufferedPath = new List<Vector2Int>();
    public float aggroRange = 10;



    void Start()
    {

        hero = mapGen.GetComponent<MapGen>().hero;
        occupiedlist = mapGen.GetComponent<MapGen>().occupiedlist;
        enemies = mapGen.GetComponent<MapGen>().enemies;
        dungeonCoords = mapGen.GetComponent<MapGen>().path;
    }   
    
    void Update()
    {

        

        Mouse mouse = Mouse.current;

        Character playerCharacter = hero.GetComponent<Character>();

        //If you have not reached the last node of your path 
        //and you are not currently moving to a node, move to the next node
        if(bufferedPath.Count > 0 && playerCharacter.GetComponent<MoveToTarget>().GetRemainingDistance() == 0)
        {
            
            playerCharacter.Move(new Vector3(bufferedPath[0].x, 0.1f, bufferedPath[0].y), occupiedlist);
            bufferedPath.RemoveAt(0);

        }else if(mouse.leftButton.wasPressedThisFrame)
        {

            GameObject target = GetComponent<ClickManager>().getObject(mouse);   

            //move player character if tile is clicked
            if(target.GetComponent<Tile>() != null && target.GetComponent<Tile>().coord != playerCharacter.coord)
            {
                    
                List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, target.GetComponent<Tile>().coord, dungeonCoords);                
                
                playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);                
                
                //If there are no enemies alerted to your presence, automatically walk entire path to destiniation
                if(aggroEnemies.Count == 0)
                {
                    pathToDestination.RemoveAt(0);
                    pathToDestination.RemoveAt(0);
                    bufferedPath = pathToDestination;
                }              
            }

            //initiate an attack on clicked enemy
            //attack if adjacent to enemy
            //move towards enemy if not adjacent
            if(target.GetComponent<Character>() != null && target.GetComponent<Character>() != playerCharacter)
            {

                Character targetCharacter = target.GetComponent<Character>();
            
                if(PathFinder.GetNeighbors(targetCharacter.coord, dungeonCoords).Contains(playerCharacter.coord))
                {

                    playerCharacter.Attack(targetCharacter);

                    //kills target of attack if it's health falls below 1
                    if(targetCharacter.health <= 0)
                    {
                        
                        aggroEnemies.Remove(target);
                        occupiedlist.Remove(targetCharacter.pos);
                        enemies.Remove(target);
                        target.GetComponent<TextPopup>().CleanUp();
                        Destroy(target);                                                                    
                    }

                }else
                {

                    List<Vector2Int> pathToDestination = PathFinder.FindPath(playerCharacter.coord, targetCharacter.coord, dungeonCoords);            
                    playerCharacter.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);
                } 
            }            

            //give a turn to each aggroed enemy
            foreach(GameObject enemy in aggroEnemies)
            {

                Character nonPlayerCharacter = enemy.GetComponent<Character>();

                //enemy attack player character if they are in a neighboring tile
                if(PathFinder.GetNeighbors(playerCharacter.coord, dungeonCoords).Contains(nonPlayerCharacter.coord))
                {
                    nonPlayerCharacter.Attack(playerCharacter);

                    if(playerCharacter.health <= 0) //kills player if their health falls below 1
                    {
                        
                        Destroy(playerCharacter);
                        print("Game Over");                                            
                    }

                }else //enemy move towards player or attack if they are in a neighboring tile                 
                {

                    List<Vector2Int> pathToPlayer = PathFinder.FindPath(nonPlayerCharacter.coord, playerCharacter.coord, dungeonCoords); 

                    if(pathToPlayer.Count > 1)
                    {

                        if(!nonPlayerCharacter.Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), occupiedlist))
                        {                  
                
                            foreach(Vector2Int v in NeighborVals.allDirectionsList)
                            {

                                List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(nonPlayerCharacter.coord, playerCharacter.coord + v, dungeonCoords);
                                
                                if(pathToPlayerPlusV != null)
                                {

                                    if(nonPlayerCharacter.Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), occupiedlist))
                                    {

                                        break;
                                    }
                                }
                            }        
                        } 
                    } 
                }
            }                       
        } 

        //aggro enemies within aggroRange units            
        foreach(GameObject enemy in enemies)
        {

            if(aggroRange > Vector3.Distance(enemy.transform.position, hero.transform.position) && !aggroEnemies.Contains(enemy))
            {

                aggroEnemies.Add(enemy);
                bufferedPath.Clear();
            }
        } 
    }   
}
