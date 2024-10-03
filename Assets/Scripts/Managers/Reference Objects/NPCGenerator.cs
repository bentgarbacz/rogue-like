using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{

    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject skeletonArcher;
    [SerializeField] private GameObject goblin;
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject witch;
    [SerializeField] private GameObject goatMan;
    private readonly float spawnPosVertOffset = 0.1f;

    public void CreateChest(Vector3 spawnPos, DungeonManager dum)
    {
        spawnPos.y += spawnPosVertOffset;

        GameObject newChest = Instantiate(chest, spawnPos, chest.transform.rotation);
        dum.AddGameObject(newChest);
        newChest.GetComponent<Loot>().coord = new Vector2Int((int)spawnPos.x, (int)spawnPos.z);
    }

    public void CreateNPC(string npcName, Vector3 spawnPos, DungeonManager dum)
    {

        spawnPos.y += spawnPosVertOffset;
        GameObject enemy = GetPrefab(npcName, spawnPos);     

        if(enemy != null)
        { 

            dum.AddGameObject(enemy);
            enemy.GetComponent<CharacterSheet>().Move(spawnPos, dum.occupiedlist);
            dum.enemies.Add(enemy);

        }else{

            Debug.LogWarning("NPC with name " + npcName + " not found.");
        }
    }

    private GameObject GetPrefab(string npcName, Vector3 spawnPos)
    {

        if(npcName == "skeleton")
        {

            return Instantiate(skeleton, spawnPos, skeleton.transform.rotation);

        }else if(npcName == "skeleton archer")
        {

            return Instantiate(skeletonArcher, spawnPos, skeletonArcher.transform.rotation);

        }else if(npcName == "goblin")
        {

            return Instantiate(goblin, spawnPos, goblin.transform.rotation);

        }else if(npcName == "rat")
        {

            return Instantiate(rat, spawnPos, rat.transform.rotation);

        }else if(npcName == "slime")
        {

            return Instantiate(slime, spawnPos, slime.transform.rotation);

        }else if(npcName == "witch")
        {

            return Instantiate(witch, spawnPos, witch.transform.rotation);

        }else if(npcName == "goatman")
        {

            return Instantiate(goatMan, spawnPos, goatMan.transform.rotation);

        }else
        {

            return null;
        }

    }
}