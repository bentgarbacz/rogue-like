using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    public GameObject hero;
    public GameObject mainCamera;
    public bool enemiesOnLookout = true;
    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<Vector2Int> discoveredCoords;
    public HashSet<Vector3> occupiedlist = new();
    public HashSet<GameObject> dungeonSpecificGameObjects = new();
    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> aggroEnemies = new();
    public HashSet<Loot> itemContainers = new();
    public List<Vector2Int> bufferedPath = new();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new();    
    private CombatManager cbm;
    private TurnSequencer ts;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();
        ts = managers.GetComponent<TurnSequencer>();

        mainCamera.GetComponent<PlayerCamera>().SetFocalPoint(hero);
    }

    public void TriggerStatusEffects()
    {

        hero.GetComponent<CharacterSheet>().GetComponent<StatusEffectManager>().ProcessStatusEffects();

        foreach(GameObject enemy in enemies)
        {

            enemy.GetComponent<CharacterSheet>().GetComponent<StatusEffectManager>().ProcessStatusEffects();
        }
    }

    public void AddGameObject(GameObject newGameObject)
    {

        dungeonSpecificGameObjects.Add(newGameObject);
    }

    public void Smite(GameObject target, Vector3 targetPosition)
    {
        //Attacks to and from dead combatants removed from combat buffer
        for(int i = 1; i < cbm.combatBuffer.Count; i++ )
        {

            if(target == cbm.combatBuffer[i].defender || target == cbm.combatBuffer[i].attacker)
            {
                
                cbm.combatBuffer.RemoveAt(i);
                i--;
            }
        }

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
            hero.GetComponent<PlayerCharacterSheet>().GainXP(gainedXP);
        }        

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

        //This is weird
        hero.GetComponent<CharacterSheet>().Teleport(new Vector3(0, 0, 0), gameObject.GetComponent<DungeonManager>());
    }

    public bool CheckPosForOccupancy(Vector3 pos)
    {

        foreach(Vector3 checkPos in occupiedlist)
        {
            
            if(Rules.PosToCoord(pos) == Rules.PosToCoord(checkPos))
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
