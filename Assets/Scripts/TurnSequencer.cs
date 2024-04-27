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
        
            Tile t = GetComponent<ClickManager>().getTile(mouse);
            
            List<Vector2Int> p = PathFinder.FindPath(new Vector2Int((int)hero.GetComponent<PlayerCharacter>().pos.x, (int)hero.GetComponent<PlayerCharacter>().pos.z), t.coord, path);
            print(p[1]);
            //hero.transform.position = new Vector3(p[1].x, 0, p[1].y);
            hero.GetComponent<PlayerCharacter>().pos = new Vector3(p[1].x, 0.5f, p[1].y); 
        }        
    }   
}



