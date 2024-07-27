using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    public GameObject hero;
    public GameObject mainCamera;
    public HashSet<Vector2Int> dungeonCoords;
    public HashSet<Vector3> occupiedlist = new();
    public HashSet<GameObject> dungeonSpecificGameObjects = new();
    public HashSet<GameObject> enemies = new();
    public HashSet<GameObject> aggroEnemies = new();
    public HashSet<Loot> itemContainers = new();
    public List<Vector2Int> bufferedPath = new();
    public Dictionary<string, List<Vector2Int>> cachedPathsDict = new();    
    private CombatManager cbm;

    void Start()
    {

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();

        mainCamera.GetComponent<PlayerCamera>().SetFocalPoint(hero);
    }

    public void TriggerStatusEffects()
    {

        hero.GetComponent<Character>().GetComponent<StatusEffectManager>().ProcessStatusEffects();

        foreach(GameObject enemy in new HashSet<GameObject>(enemies))
        {

            enemy.GetComponent<Character>().GetComponent<StatusEffectManager>().ProcessStatusEffects();
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

        target.GetComponent<DropLoot>().Drop();

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
