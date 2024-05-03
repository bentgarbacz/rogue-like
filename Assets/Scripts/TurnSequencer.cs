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



    void Start()
    {

        hero = mapGen.GetComponent<MapGen>().hero;
        occupiedlist = mapGen.GetComponent<MapGen>().occupiedlist;
        enemies = mapGen.GetComponent<MapGen>().enemies;
    }   
    
    void Update()
    {

        dungeonCoords = mapGen.GetComponent<MapGen>().path;

        Mouse mouse = Mouse.current;

        Character pc = hero.GetComponent<Character>();

        //If you have not reached the last node of your path 
        //and you are not currently moving to a node, move to the next node
        if(bufferedPath.Count > 0 && pc.GetComponent<MoveToTarget>().GetRemainingDistance() == 0)
        {
            
            pc.Move(new Vector3(bufferedPath[0].x, 0.1f, bufferedPath[0].y), occupiedlist);
            bufferedPath.RemoveAt(0);

        }else if(mouse.leftButton.wasPressedThisFrame)
        {

            GameObject target = GetComponent<ClickManager>().getObject(mouse);   

            //move player character if tile is clicked
            if(target.GetComponent<Tile>() != null && target.GetComponent<Tile>().coord != pc.coord)
            {
                    
                List<Vector2Int> pathToDestination = PathFinder.FindPath(pc.coord, target.GetComponent<Tile>().coord, dungeonCoords);                
                
                pc.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);                
                
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
            if(target.GetComponent<Character>() != null && target.GetComponent<Character>() != pc)
            {

                Character tc = target.GetComponent<Character>();
            
                if(PathFinder.GetNeighbors(tc.coord, dungeonCoords).Contains(pc.coord))
                {

                    pc.Attack(tc);

                    //kills target of attack if it's health falls below 1
                    if(tc.health <= 0)
                    {
                        
                        aggroEnemies.Remove(target);
                        occupiedlist.Remove(tc.pos);
                        enemies.Remove(target);
                        Destroy(target);                                                                    
                    }

                }else
                {

                    List<Vector2Int> pathToDestination = PathFinder.FindPath(pc.coord, tc.coord, dungeonCoords);            
                    pc.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);
                } 
            }            

            //give a turn to each aggroed enemy
            foreach(GameObject e in aggroEnemies)
            {

                Character npc = e.GetComponent<Character>();

                //enemy attack player character if they are in a neighboring tile
                if(PathFinder.GetNeighbors(pc.coord, dungeonCoords).Contains(npc.coord))
                {
                    npc.Attack(pc);

                    if(pc.health <= 0) //kills player if their health falls below 1
                    {
                        
                        Destroy(pc);
                        print("Game Over");                                            
                    }

                }else //enemy move towards player or attack if they are in a neighboring tile                 
                {

                    List<Vector2Int> pathToPlayer = PathFinder.FindPath(npc.coord, pc.coord, dungeonCoords); 

                    if(!npc.Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), occupiedlist))
                    {                  
            
                        foreach(Vector2Int v in NeighborVals.allDirectionsList)
                        {

                            List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(npc.coord, pc.coord + v, dungeonCoords);

                            if(npc.Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), occupiedlist))
                            {

                                break;
                            }
                        }        
                    }  
                }
            }                       
        } 

        //aggro enemies within 10 units            
        foreach(GameObject e in enemies)
        {

            if(10 > Vector3.Distance(e.transform.position, hero.transform.position) && !aggroEnemies.Contains(e))
            {

                aggroEnemies.Add(e);
                bufferedPath.Clear();
            }
        } 
    }   
}
