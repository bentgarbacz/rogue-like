using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    public GameObject hero;
    public PlayerCharacterSheet playerCharacter;
    public bool enemiesOnLookout = true;
    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<Vector2Int> occupiedlist = new();
    public HashSet<GameObject> dungeonSpecificGameObjects = new();
    //public HashSet<GameObject> iconGameObjects = new();
    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> aggroEnemies = new();
    public HashSet<Loot> itemContainers = new();
    private CombatManager cbm;
    private TurnSequencer ts;
    private MiniMapManager miniMapManager;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private VisibilityManager visibilityManager;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();
        ts = managers.GetComponent<TurnSequencer>();
        miniMapManager = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
    }

    public void TriggerStatusEffects()
    {

        playerCharacter.ProcessStatusEffects();

        foreach (GameObject enemy in enemies.ToList())
        {

            enemy.GetComponent<CharacterSheet>().ProcessStatusEffects();
        }
    }

    public void AddGameObject(GameObject newGameObject)
    {

        dungeonSpecificGameObjects.Add(newGameObject);
        tileManager.AddTile(newGameObject.GetComponent<Tile>());
        tileManager.AddDoor(newGameObject.GetComponent<Door>());
        visibilityManager.AddObject(newGameObject);
    }

    public void Smite(GameObject target, Vector2Int targetCoord)
    {
        //Attacks to and from dead combatants removed from combat buffer
        cbm.PruneCombatBuffer(target);

        aggroEnemies.Remove(target);
        occupiedlist.Remove(targetCoord);
        enemies.Remove(target);
        visibilityManager.RemoveObject(target);

        if (target.GetComponent<PlayerCharacterSheet>())
        {

            HaltGameplay();

        }
        else
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

        if (!trashContainer.GetComponent<Chest>())
        {

            visibilityManager.RemoveObject(trashContainer);
            dungeonSpecificGameObjects.Remove(trashContainer);
            Destroy(trashContainer);
            miniMapManager.UpdateDynamicIcons();
        }
    }

    public void CleanUp()
    {

        enemiesOnLookout = true;
        dungeonCoords = new();
        enemies = new();
        occupiedlist = new();
        itemContainers = new();

        foreach (GameObject trash in dungeonSpecificGameObjects)
        {

            Destroy(trash);
        }

        foreach (GameObject trash in miniMapManager.iconGameObjects)
        {

            Destroy(trash);
        }

        dungeonSpecificGameObjects = new();
        aggroEnemies = new();

        tileManager.RefreshLayout();
        visibilityManager.Refresh();

        playerCharacter.Teleport(new Vector2Int(0, 0), this);
    }

    public void ClearAggroBuffer()
    {

        foreach (GameObject enemy in aggroEnemies)
        {

            enemy.GetComponent<TextNotificationManager>().CreateNotificationOrder(enemy.transform.position, 2, "?", Color.red);
        }

        aggroEnemies = new HashSet<GameObject>();
    }

    public void HaltGameplay(bool isHalted = true)
    {

        ts.gameplayHalted = isHalted;
    }
}
