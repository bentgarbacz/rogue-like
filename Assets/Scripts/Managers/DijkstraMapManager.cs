using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class DijkstraMapManager : MonoBehaviour
{

    [SerializeField] private EntityManager entityMgr;
    [SerializeField] private TileManager tileMgr;
    public Dictionary<Vector2Int, float> playerMap = new();
    public Dictionary<Vector2Int, float> enemyMap = new();
    public Dictionary<Vector2Int, float> lootMap = new();
    public Dictionary<Vector2Int, float> npcMap = new();
    public Dictionary<Vector2Int, float> playerAndNpcMap = new();


    public Dictionary<Vector2Int, float> LayerMaps(List<Dictionary<Vector2Int, float>> maps)
    {

        Dictionary<Vector2Int, float> layeredMap = new();

        foreach (Dictionary<Vector2Int, float> map in maps)
        {

            foreach (KeyValuePair<Vector2Int, float> kvPair in map)
            {

                if (layeredMap.ContainsKey(kvPair.Key))
                {

                    if (layeredMap[kvPair.Key] < kvPair.Value)
                    {

                        continue;
                    }
                }

                layeredMap[kvPair.Key] = kvPair.Value;
            }
        }

        return layeredMap;
    }

    public Dictionary<Vector2Int, float> CreateMapAboutObject(GameObject gameObject, int maxSteps)
    {

        ObjectLocation rootLocation = gameObject.GetComponent<ObjectLocation>();

        if (!rootLocation)
        {

            return null;
        }

        Dictionary<Vector2Int, float> objectMap = new();
        HashSet<Vector2Int> neighbors = new() { rootLocation.coord };
        int currentStep = 0;

        objectMap[rootLocation.coord] = 0f;

        while (currentStep < maxSteps && neighbors.Count > 0)
        {

            HashSet<Vector2Int> nextNeighbors = new();

            foreach (Vector2Int coord in neighbors)
            {

                //objectMap[coord] = (float)currentStep;

                foreach (Vector2Int checkCoord in PathFinder.GetNeighbors(coord, tileMgr.levelCoords))
                {

                    if (objectMap.ContainsKey(checkCoord))
                    {

                        continue;
                    }

                    objectMap[checkCoord] = currentStep + 1;
                    nextNeighbors.Add(checkCoord);
                }

            }

            neighbors = nextNeighbors;
            currentStep += 1;
        }


        return objectMap;
    }

    public Dictionary<Vector2Int, float> CreateLayeredMap(HashSet<GameObject> gameObjects, int maxSteps)
    {

        List<Dictionary<Vector2Int, float>> maps = new();

        foreach (GameObject obj in gameObjects)
        {
            
            Dictionary<Vector2Int, float> map = CreateMapAboutObject(obj, maxSteps);

            if (map != null)
            {

                maps.Add(map);
            }
        }

        return LayerMaps(maps);
    }

    public void PopulatePlayerMap()
    {

        playerMap = CreateMapAboutObject(entityMgr.hero, 25);
        //string filePath = @"C:\Users\bentg\Downloads\PLAYERmap_output.txt";
        //PrintMapToFile(playerMap, filePath);
    }

    public void PopulateEnemyMap()
    {

        enemyMap = CreateLayeredMap(entityMgr.enemies, 10);
        //string filePath = @"C:\Users\bentg\Downloads\ENEMYmap_output.txt";
        //PrintMapToFile(enemyMap, filePath);
    }

    public void PopulateLootMap()
    {

        //LootMap = CreateLayeredMap(dum.itemContainers, 50);
    }

    public void PopulateNPCMap()
    {

        npcMap = CreateLayeredMap(entityMgr.npcs, 10);
        //string filePath = @"C:\Users\bentg\Downloads\NPCmap_output.txt";
        //PrintMapToFile(npcMap, filePath);
    }
    
    public void UpdateCombinedMapPlayerAndNPC()
    {
        
        List<Dictionary<Vector2Int, float>> maps = new() {npcMap, playerMap};
        playerAndNpcMap = LayerMaps(maps);
    }

    public float GetPlayerMapValue(Vector2Int coord)
    {
        if (playerMap != null && playerMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public float GetEnemyMapValue(Vector2Int coord)
    {
        if (enemyMap != null && enemyMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public float GetNpcMapValue(Vector2Int coord)
    {
        if (npcMap != null && npcMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public float GetLootMapValue(Vector2Int coord)
    {
        if (lootMap != null && lootMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public void PrintMapToFile(Dictionary<Vector2Int, float> map, string filePath)
    {
        if (map == null || map.Count == 0)
        {
            File.WriteAllText(filePath, "Map is empty." + System.Environment.NewLine);
            return;
        }

        int minX = map.Keys.Min(coord => coord.x);
        int maxX = map.Keys.Max(coord => coord.x);
        int minY = map.Keys.Min(coord => coord.y);
        int maxY = map.Keys.Max(coord => coord.y);

        using StreamWriter writer = new StreamWriter(filePath, false);
        for (int y = maxY; y >= minY; y--)
        {
            string line = "";
            for (int x = minX; x <= maxX; x++)
            {
                Vector2Int coord = new Vector2Int(x, y);
                if (map.ContainsKey(coord))
                {
                    line += ((int)map[coord]).ToString().PadLeft(2, '0') + " ";
                }
                else
                {
                    line += " . ";
                }
            }
            writer.WriteLine(line);
        }
    }
}
