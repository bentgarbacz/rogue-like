using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public GameObject hero;
    public HashSet<Vector2Int> path;
    public HashSet<GameObject> aggroEnemies = new HashSet<GameObject>();
    public HashSet<Vector3> occupiedlist;

    void Start()
    {

        hero = mapGen.GetComponent<MapGen>().hero;
        occupiedlist = mapGen.GetComponent<MapGen>().occupiedlist;
    }
 
    void Update()
    {
        
        path = mapGen.GetComponent<MapGen>().path;

        Mouse mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
        
            Tile targetTile = GetComponent<ClickManager>().getTile(mouse);
            PlayerCharacter pc = hero.GetComponent<PlayerCharacter>();
            
            List<Vector2Int> pathToDestination = PathFinder.FindPath(new Vector2Int((int)pc.pos.x, (int)pc.pos.z), targetTile.coord, path);
            
            pc.Move(new Vector3(pathToDestination[1].x, 0.1f, pathToDestination[1].y), occupiedlist); 

            foreach(GameObject e in mapGen.GetComponent<MapGen>().enemies)
            {

                if(10 > Vector3.Distance(e.transform.position, hero.transform.position) && !aggroEnemies.Contains(e))
                {

                    aggroEnemies.Add(e);
                }
            }

            foreach(GameObject e in aggroEnemies)
            {

                PlayerCharacter npc = e.GetComponent<PlayerCharacter>();            
                List<Vector2Int> pathToPlayer = PathFinder.FindPath(new Vector2Int((int)npc.pos.x, (int)npc.pos.z), new Vector2Int((int)pc.pos.x, (int)pc.pos.z), path);            
                npc.Move(new Vector3(pathToPlayer[1].x, 0.1f, pathToPlayer[1].y), occupiedlist);                
            }
        }        
    }   
}



