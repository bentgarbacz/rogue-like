using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public GameObject hero;
    public PlayerCharacterSheet playerCharacter;
    public bool enemiesOnLookout = true;
    public HashSet<GameObject> entitiesInLevel = new();
    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> aggroEnemies = new();
    public HashSet<GameObject> npcs = new();
    public HashSet<Loot> itemContainers = new();
    [SerializeField] private CombatManager combatMgr;
    [SerializeField] private TurnSequencer ts;
    [SerializeField] private MiniMapManager minimapMgr;
    [SerializeField] private TileManager tileMgr;
    [SerializeField] private VisibilityManager visibilityMgr;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        
        minimapMgr = managers.GetComponent<UIActiveManager>().mapPanel.GetComponent<MiniMapManager>();
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

        entitiesInLevel.Add(newGameObject);
        tileMgr.AddTile(newGameObject.GetComponent<Tile>());
        tileMgr.AddDoor(newGameObject.GetComponent<Door>());
        visibilityMgr.AddObject(newGameObject);
    }

    public void Smite(GameObject target)
    {
        //Attacks to and from dead combatants removed from combat buffer
        combatMgr.PruneCombatBuffer(target);

        aggroEnemies.Remove(target);
        tileMgr.occupiedlist.Remove(target.GetComponent<ObjectLocation>().coord);
        enemies.Remove(target);
        npcs.Remove(target);
        visibilityMgr.RemoveObject(target);

        if (target.GetComponent<PlayerCharacterSheet>())
        {

            ts.HaltGameplay();

        }
        else
        {

            target.GetComponent<DropLoot>().Drop();

            int gainedXP = target.GetComponent<CharacterSheet>().level * 5;
            playerCharacter.GainXP(gainedXP);
        }

        Destroy(target);
        minimapMgr.UpdateDynamicIcons();
    }

    public void TossContainer(GameObject trashContainer)
    {

        if (trashContainer.GetComponent<Chest>())
        {

            return;
        }

        visibilityMgr.RemoveObject(trashContainer);
        entitiesInLevel.Remove(trashContainer);
        Destroy(trashContainer);
        minimapMgr.UpdateDynamicIcons();
    }

    public void CleanUp()
    {

        enemiesOnLookout = true;
        tileMgr.levelCoords = new();
        enemies = new();
        tileMgr.occupiedlist = new();
        itemContainers = new();

        foreach (GameObject trash in entitiesInLevel)
        {

            Destroy(trash);
        }

        foreach (GameObject trash in minimapMgr.iconGameObjects)
        {

            Destroy(trash);
        }

        entitiesInLevel = new();
        aggroEnemies = new();
        npcs = new();

        tileMgr.RefreshLayout();
        visibilityMgr.Refresh();

        playerCharacter.Teleport(new Vector2Int(0, 0));
    }

    public void ClearAggroBuffer()
    {

        foreach (GameObject enemy in aggroEnemies)
        {

            enemy.GetComponent<TextNotificationManager>().CreateNotificationOrder(enemy.transform.position, 2, "?", Color.red);
        }

        aggroEnemies = new HashSet<GameObject>();
    }
}
