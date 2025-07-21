using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{

    private Dictionary<NPCType, GameObject> npcDict;
    [SerializeField] private DungeonManager dum;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject skeletonArcher;
    [SerializeField] private GameObject goblin;
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject witch;
    [SerializeField] private GameObject goatMan;
    [SerializeField] private GameObject spider;
    [SerializeField] private GameObject skull;
    private readonly Vector3 spawnPosOffset = new(0, 0.1f, 0);

    void Start()
    {

        npcDict = new()
        {

            {NPCType.Chest, chest},
            {NPCType.Skeleton, skeleton},
            {NPCType.SkeletonArcher, skeletonArcher},
            {NPCType.Goblin, goblin},
            {NPCType.Rat, rat},
            {NPCType.Slime, slime},
            {NPCType.Witch, witch},
            {NPCType.GoatMan, goatMan},
            {NPCType.Spider, spider},
            {NPCType.Skull, skull}
        };
    }

    public void CreateChest(Vector3 spawnPos)
    {

        int yRotation = Random.Range(0, 8) * 45;
        Quaternion spawnQuart = Quaternion.Euler(0, yRotation, 0);

        GameObject newChest = Instantiate(chest, spawnPos + spawnPosOffset, spawnQuart);
        newChest.GetComponent<ObjectVisibility>().Initialize();
        dum.AddGameObject(newChest);

        Loot newLoot = newChest.GetComponent<Loot>();
        newLoot.loc.coord = new Vector2Int((int)spawnPos.x, (int)spawnPos.z);
        tileManager.tileDict[newLoot.loc.coord].EntitiesOnTile.Add(newChest);
    }

    public GameObject CreateNPC(NPCType npcType, Vector3 spawnPos)
    {

        GameObject enemy = GetPrefab(npcType, spawnPos + spawnPosOffset);
        enemy.GetComponent<ObjectVisibility>().Initialize();
        Vector2Int spawnCoord = new((int)spawnPos.x, (int)spawnPos.z);

        if (enemy != null)
        {

            dum.AddGameObject(enemy);
            enemy.GetComponent<CharacterSheet>().Move(spawnCoord, dum.occupiedlist);
            dum.enemies.Add(enemy);

            return enemy;
        }

        return null;
    }

    private GameObject GetPrefab(NPCType npcType, Vector3 spawnPos)
    {

        if (npcDict.TryGetValue(npcType, out GameObject npcPrefab))
        {

            return Instantiate(npcPrefab, spawnPos, npcPrefab.transform.rotation);

        }else{

            return null;
        }
    }
}