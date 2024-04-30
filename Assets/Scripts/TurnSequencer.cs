using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public GameObject hero;
    public HashSet<Vector2Int> path;
    public HashSet<GameObject> enemies = new HashSet<GameObject>();
    public HashSet<GameObject> aggroEnemies = new HashSet<GameObject>();
    public HashSet<Vector3> occupiedlist;
    

    void Start()
    {

        hero = mapGen.GetComponent<MapGen>().hero;
        occupiedlist = mapGen.GetComponent<MapGen>().occupiedlist;
        enemies = mapGen.GetComponent<MapGen>().enemies;
    }
 
    void Update()
    {

        path = mapGen.GetComponent<MapGen>().path;

        Mouse mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {

            GameObject target = GetComponent<ClickManager>().getObject(mouse);   
            Character pc = hero.GetComponent<Character>();

            //move player character if tile is clicked
            if(target.GetComponent<Tile>()){

                List<Vector2Int> pathToDestination = PathFinder.FindPath(pc.coord, target.GetComponent<Tile>().coord, path);            
                pc.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);
            }

            //initiate an attack on clicked enemy
            //attack if adjacent to enemy
            //move towards enemy if not adjacent
            if(target.GetComponent<Character>())
            {

                Character tc = target.GetComponent<Character>();
                
                if(PathFinder.GetNeighbors(tc.coord, path).Contains(pc.coord))
                {

                    pc.attack(target.GetComponent<Character>());

                    //kills target of attack if it's health falls below 1
                    if(target.GetComponent<Character>().health <= 0)
                    {
                        
                        aggroEnemies.Remove(target);
                        occupiedlist.Remove(tc.pos);
                        enemies.Remove(target);
                        Destroy(target);
                                                                    
                    }

                }else
                {

                    List<Vector2Int> pathToDestination = PathFinder.FindPath(pc.coord, tc.coord, path);            
                    pc.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist);
                }
            }

            //aggroed enemies attack player character if they are in a neighboring tile

            //aggroed enemies move towards player
            foreach(GameObject e in aggroEnemies)
            {

                Character npc = e.GetComponent<Character>();  
                
                List<Vector2Int> pathToPlayer = PathFinder.FindPath(npc.coord, pc.coord, path);            
                if(!npc.Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), occupiedlist))
                {                  
        
                    foreach(Vector2Int v in NeighborVals.allDirectionsList)
                    {

                        List<Vector2Int> pathToPlayerPlusV = PathFinder.FindPath(npc.coord, pc.coord + v, path);

                        if(npc.Move(new Vector3(pathToPlayerPlusV[1].x, 0.1f, pathToPlayerPlusV[1].y), occupiedlist))
                        {
                            break;
                        }
                    }        
                }                              
            }

            //aggro enemies within 10 units            
            foreach(GameObject e in enemies)
            {

                if(10 > Vector3.Distance(e.transform.position, hero.transform.position) && !aggroEnemies.Contains(e))
                {
                    print("adding");
                    print(aggroEnemies.Count);
                    aggroEnemies.Add(e);
                    print(aggroEnemies.Count);
                }
            }
            
        } 
    }   
}



