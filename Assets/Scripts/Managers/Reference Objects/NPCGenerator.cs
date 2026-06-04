using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{

    private Dictionary<NPCType, GameObject> npcDict;
    [SerializeField] private EntityManager entityMgr;
    [SerializeField] private TileManager tileMgr;
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject mimicChest;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject skeletalRemains;
    [SerializeField] private GameObject skeletonArcher;
    [SerializeField] private GameObject goblin;
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject witch;
    [SerializeField] private GameObject goatMan;
    [SerializeField] private GameObject spider;
    [SerializeField] private GameObject skull;
    [SerializeField] private GameObject floater;
    [SerializeField] private GameObject stoneGolem;
    [SerializeField] private GameObject lich;
    [SerializeField] private GameObject trap;
    [SerializeField] private GameObject mimic;
    private readonly int totalModifiers = 6;
    private readonly Vector3 spawnPosOffset = new(0, 0.1f, 0);

    void Start()
    {

        npcDict = new()
        {

            {NPCType.Chest, chest},
            {NPCType.MimicChest, mimicChest},
            {NPCType.Skeleton, skeleton},
            {NPCType.SkeletalRemains, skeletalRemains},
            {NPCType.SkeletonArcher, skeletonArcher},
            {NPCType.Goblin, goblin},
            {NPCType.Rat, rat},
            {NPCType.Slime, slime},
            {NPCType.Witch, witch},
            {NPCType.GoatMan, goatMan},
            {NPCType.Spider, spider},
            {NPCType.Skull, skull},
            {NPCType.Floater, floater},
            {NPCType.StoneGolem, stoneGolem},
            {NPCType.Lich, lich},
            {NPCType.Trap, trap},
            {NPCType.Mimic, mimic}
        };
    }

    //The int passed to mimic chance is treated as a % chance.
    //  The default 5 value is treated as 5% chance to occur.
    //  Values below 0 and above 100 are treated as 0% and 100% respectively.
    public void CreateChest(Vector3 spawnPos, int mimicChance = 100)
    {

        GameObject newChest;

        int yRotation = Random.Range(0, 8) * 45;
        Quaternion spawnQuart = Quaternion.Euler(0, yRotation, 0);

        if(mimicChance > Random.Range(0, 100))
        {

            newChest = Instantiate(mimicChest, spawnPos + spawnPosOffset, spawnQuart);
        }
        else
        {
            
            newChest = Instantiate(chest, spawnPos + spawnPosOffset, spawnQuart);
        }

        newChest.GetComponent<ObjectVisibility>().Initialize();
        entityMgr.AddGameObject(newChest);

        Interactable newLoot = newChest.GetComponent<Interactable>();
        newLoot.loc.coord = new Vector2Int((int)spawnPos.x, (int)spawnPos.z);
        tileMgr.GetTile(newLoot.loc.coord).AddEntity(newChest);
    }

    public GameObject CreateEnemy(NPCType enemyType, Vector3 spawnPos)
    {

        Vector2Int spawnCoord = new((int)spawnPos.x, (int)spawnPos.z);

        if (tileMgr.occupiedlist.Contains(spawnCoord))
        {

            return null;
        }

        GameObject enemy = GetPrefab(enemyType, spawnPos + spawnPosOffset);
        enemy.GetComponent<ObjectVisibility>().Initialize(); 
        
        tileMgr.occupiedlist.Add(spawnCoord);
        tileMgr.GetTile(spawnCoord).AddEntity(enemy);

        entityMgr.AddGameObject(enemy);
        entityMgr.enemies.Add(enemy);

        ApplyRandomNPCModifiers(enemy);

        return enemy;
    }
    
    public GameObject CreateNPC(NPCType npcType, Vector3 spawnPos)
    {

        Vector2Int spawnCoord = new((int)spawnPos.x, (int)spawnPos.z);

        if (tileMgr.occupiedlist.Contains(spawnCoord))
        {

            return null;
        }

        GameObject npc = GetPrefab(npcType, spawnPos + spawnPosOffset);
        npc.GetComponent<ObjectVisibility>().Initialize();
        
        tileMgr.occupiedlist.Add(spawnCoord);
        tileMgr.GetTile(spawnCoord).AddEntity(npc);

        entityMgr.AddGameObject(npc);
        entityMgr.friendlies.Add(npc);

        return npc;

    }

    public GameObject CreateTrap(Vector3 spawnPos)
    {

        Vector2Int spawnCoord = new((int)spawnPos.x, (int)spawnPos.z);

        GameObject trapObj = GetPrefab(NPCType.Trap, spawnPos + spawnPosOffset);

        if (tileMgr.occupiedlist.Contains(spawnCoord))
        {
            return null;
        }

        trapObj.GetComponent<ObjectVisibility>().Initialize();
        
        // Traps don't block tiles, but they are still added to the level
        tileMgr.GetTile(spawnCoord).AddEntity(trapObj);
        entityMgr.AddGameObject(trapObj);
        entityMgr.inanimates.Add(trapObj);

        return trapObj;

    }

    private void ApplyRandomNPCModifiers(GameObject npc)
    {

        CharacterSheet characterSheet = npc.GetComponent<CharacterSheet>();
        //ApplyModifierByIndex(0, characterSheet.characterModMgr, characterSheet);
        //return;

        // Randomly determine 0-3 modifiers to apply
        int newModifierCount = Random.Range(0, 4);

        // Choose unique indices
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < newModifierCount)
        {
            selectedIndices.Add(Random.Range(0, totalModifiers));
        }

        // Apply the selected modifiers
        foreach (int index in selectedIndices)
        {
            ApplyModifierByIndex(index, characterSheet.characterModMgr, characterSheet);
        }
    }

    private void ApplyModifierByIndex(int index, CharacterModifierManager modifierManager, CharacterSheet characterSheet)
    {

        CharacterModifier newModifier = index switch
        {
            0 => new Confusing(characterSheet),
            1 => new Nimble(characterSheet),
            2 => new Poisonous(characterSheet),
            3 => new Precise(characterSheet),
            4 => new Stout(characterSheet),
            5 => new Piercing(characterSheet),
            _ => null
        };

        if (newModifier != null)
        {
            modifierManager.AddModifier(newModifier);
        }
    }

    private GameObject GetPrefab(NPCType npcType, Vector3 spawnPos)
    {

        if (npcDict.TryGetValue(npcType, out GameObject npcPrefab))
        {

            return Instantiate(npcPrefab, spawnPos, npcPrefab.transform.rotation);

        }
        else
        {

            return null;
        }
    }
}
