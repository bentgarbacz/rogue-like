using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    public GameObject hero;
    public GameObject mainCamera;
    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<Vector3> occupiedlist = new HashSet<Vector3>();
    public HashSet<GameObject> dungeonSpecificGameObjects = new HashSet<GameObject>();
    public HashSet<GameObject> enemies = new HashSet<GameObject>();
    public HashSet<GameObject> aggroEnemies = new HashSet<GameObject>();
    public HashSet<Loot> itemContainers = new HashSet<Loot>();
    public List<Vector2Int> bufferedPath = new List<Vector2Int>();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new Dictionary<string, List<Vector2Int>>();    

    void Start()
    {

        mainCamera.GetComponent<PlayerCamera>().SetFocalPoint(hero);
    }

    public void AddGameObject(GameObject newGameObject)
    {

        dungeonSpecificGameObjects.Add(newGameObject);
    }

    public void Smite(GameObject target, Vector3 targetPosition)
    {

        aggroEnemies.Remove(target);
        occupiedlist.Remove(targetPosition);
        enemies.Remove(target);

        target.GetComponent<DropLoot>().Drop();
        target.GetComponent<TextPopup>().CleanUp();

        int gainedXP = target.GetComponent<Character>().level * 5;
        hero.GetComponent<PlayerCharacter>().GainXP(gainedXP);

        Destroy(target);  
    }

    public void TossContainer(GameObject trashContainer)
    {
        if(!trashContainer.GetComponent<Chest>())
        {
            
            dungeonSpecificGameObjects.Remove(trashContainer);
            Destroy(trashContainer);
        }
    }

    public void CleanUp()
    {
        
        cachedPathsDict = new Dictionary<string, List<Vector2Int>>();
        enemies = new HashSet<GameObject>();
        occupiedlist = new HashSet<Vector3>();
        itemContainers = new HashSet<Loot>();

        foreach(GameObject trash in dungeonSpecificGameObjects)
        {

            Destroy(trash);
        }

        dungeonSpecificGameObjects = new HashSet<GameObject>();
        aggroEnemies = new HashSet<GameObject>();
        bufferedPath = new List<Vector2Int>();

        hero.GetComponent<Character>().Teleport(new Vector3(0, 0, 0), occupiedlist);
    }
}
