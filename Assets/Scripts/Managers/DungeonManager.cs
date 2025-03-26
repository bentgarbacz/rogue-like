using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    public GameObject hero;
    public PlayerCharacterSheet playerCharacter;
    public GameObject mainCamera;
    public bool enemiesOnLookout = true;
    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<Vector2Int> discoveredCoords;
    public HashSet<Vector3> occupiedlist = new();
    public HashSet<GameObject> dungeonSpecificGameObjects = new();
    public HashSet<GameObject> iconGameObjects = new();
    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> aggroEnemies = new();
    public HashSet<Loot> itemContainers = new();
    public List<Vector2Int> bufferedPath = new();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new();    
    private CombatManager cbm;
    private TurnSequencer ts;
    private MiniMapManager miniMapManager;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();
        ts = managers.GetComponent<TurnSequencer>();
        miniMapManager = GameObject.Find("CanvasHUD").GetComponentInChildren<MiniMapManager>();
        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();

        mainCamera.GetComponent<PlayerCamera>().SetFocalPoint(hero);
    }

    public void TriggerStatusEffects()
    {

        playerCharacter.ProcessStatusEffects();

        foreach(GameObject enemy in enemies.ToList())
        {

            enemy.GetComponent<CharacterSheet>().ProcessStatusEffects();
        }
    }

    public void AddGameObject(GameObject newGameObject)
    {

        dungeonSpecificGameObjects.Add(newGameObject);
    }

    public void Smite(GameObject target, Vector3 targetPosition)
    {
        //Attacks to and from dead combatants removed from combat buffer
        cbm.PruneCombatBuffer(target);

        aggroEnemies.Remove(target);
        occupiedlist.Remove(targetPosition);
        enemies.Remove(target);

        if(target.GetComponent<PlayerCharacterSheet>())
        {

            ts.gameplayHalted = true;

        }else
        {

            target.GetComponent<DropLoot>().Drop();

            int gainedXP = target.GetComponent<CharacterSheet>().level * 5;
            playerCharacter.GainXP(gainedXP);
        }        

        Destroy(target);
        miniMapManager.UpdateDynamicIcons();
    }

    public void TossContainer(GameObject trashContainer)
    {
        
        if(!trashContainer.GetComponent<Chest>())
        {
            
            dungeonSpecificGameObjects.Remove(trashContainer);
            Destroy(trashContainer);
            miniMapManager.UpdateDynamicIcons();
        }
    }

    public void CleanUp()
    {
        
        enemiesOnLookout = true;
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

        playerCharacter.Teleport(new Vector3(0, 0, 0), this);
    }

    public bool CheckPosForOccupancy(Vector3 pos)
    {

        foreach(Vector3 checkPos in occupiedlist)
        {
            
            if(GameFunctions.PosToCoord(pos) == GameFunctions.PosToCoord(checkPos))
            {

                return true;
            }
        }

        return false;
    }

    public void ClearAggroBuffer()
    {

        foreach(GameObject enemy in aggroEnemies)
        {

            enemy.GetComponent<TextNotificationManager>().CreateNotificationOrder(enemy.transform.position, 2, "?", Color.red);
        }

        aggroEnemies = new HashSet<GameObject>();
    }
}
