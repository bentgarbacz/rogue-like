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
    public List<Vector2Int> bufferedPath = new List<Vector2Int>();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new Dictionary<string, List<Vector2Int>>();    

    void Start()
    {

        mainCamera.GetComponent<PlayerCamera>().setFocalPoint(hero);
    }

    public void AddGameObject(GameObject newGameObject)
    {

        dungeonSpecificGameObjects.Add(newGameObject);
    }

    public void CleanUp()
    {
        
        cachedPathsDict = new Dictionary<string, List<UnityEngine.Vector2Int>>();
        enemies = new HashSet<GameObject>();
        occupiedlist = new HashSet<Vector3>();

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
