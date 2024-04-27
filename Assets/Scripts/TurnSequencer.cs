using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSequencer : MonoBehaviour
{
    public GameObject mapGen;
    public GameObject hero;
    public HashSet<Vector2Int> path;
    void Start()
    {

        hero = mapGen.GetComponent<MapGen>().hero;
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

            pc.Move(new Vector3(pathToDestination[1].x, 0.5f, pathToDestination[1].y)); 
        }        
    }   
}



