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
    public HashSet<Vector2Int> occupiedlist = new();
    public HashSet<GameObject> dungeonSpecificGameObjects = new();
    public HashSet<GameObject> iconGameObjects = new();
    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> aggroEnemies = new();
    public HashSet<Loot> itemContainers = new();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new();    
    private CombatManager cbm;
    private TurnSequencer ts;
    private MiniMapManager miniMapManager;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();
        ts = managers.GetComponent<TurnSequencer>();
        miniMapManager = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
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

    /*public HashSet<Vector2Int> GetOccupiedCoords()
    {

        HashSet<Vector2Int> occupiedCoords = new();

        foreach (Vector3 occupiedPos in occupiedlist)
        {
            
            Vector2Int coord = GameFunctions.PosToCoord(occupiedPos);
            occupiedCoords.Add(coord);
        }

        return occupiedCoords;
    }*/

    public void Smite(GameObject target, Vector2Int targetCoord)
    {
        //Attacks to and from dead combatants removed from combat buffer
        cbm.PruneCombatBuffer(target);

        aggroEnemies.Remove(target);
        occupiedlist.Remove(targetCoord);
        enemies.Remove(target);

        if(target.GetComponent<PlayerCharacterSheet>())
        {

            HaltGameplay();

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
        occupiedlist = new HashSet<Vector2Int>();
        itemContainers = new HashSet<Loot>();

        foreach(GameObject trash in dungeonSpecificGameObjects)
        {

            Destroy(trash);
        }

        foreach(GameObject trash in iconGameObjects)
        {

            Destroy(trash);
        }

        dungeonSpecificGameObjects = new HashSet<GameObject>();
        aggroEnemies = new HashSet<GameObject>();

        playerCharacter.Teleport(new Vector2Int(0, 0), this);
    }

    /*public bool CheckPosForOccupancy(Vector3 pos)
    {

        foreach(Vector3 checkPos in occupiedlist)
        {
            
            if(GameFunctions.PosToCoord(pos) == GameFunctions.PosToCoord(checkPos))
            {

                return true;
            }
        }

        return false;
    }*/

    public void ClearAggroBuffer()
    {

        foreach(GameObject enemy in aggroEnemies)
        {

            enemy.GetComponent<TextNotificationManager>().CreateNotificationOrder(enemy.transform.position, 2, "?", Color.red);
        }

        aggroEnemies = new HashSet<GameObject>();
    }

    public void DeleteTile(Vector2Int coord)
    {

        foreach(GameObject checkObject in dungeonSpecificGameObjects)
        {

            if(checkObject.GetComponent<Tile>())
            {

                if(checkObject.GetComponent<Tile>().coord == coord)
                {

                    Destroy(checkObject);
                    return;
                }
            }
        }
    }

    public void HaltGameplay(bool isHalted = true)
    {

        ts.gameplayHalted = isHalted;
    }
}
