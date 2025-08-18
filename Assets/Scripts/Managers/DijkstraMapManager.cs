using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class DijkstraMapManager : MonoBehaviour
{

    [SerializeField] DungeonManager dum;
    public Dictionary<Vector2Int, float> PlayerMap { get; private set; }
    public Dictionary<Vector2Int, float> EnemyMap { get; private set; }
    public Dictionary<Vector2Int, float> LootMap { get; private set; }
    public Dictionary<Vector2Int, float> NpcMap { get; private set; }


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

                foreach (Vector2Int checkCoord in PathFinder.GetNeighbors(coord, dum.dungeonCoords))
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

        List<Dictionary<Vector2Int, float>> maps = new List<Dictionary<Vector2Int, float>>();

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

        PlayerMap = CreateMapAboutObject(dum.hero, 25);
        //string filePath = @"C:\Users\bentg\Downloads\PLAYERmap_output.txt";
        //PrintMapToFile(PlayerMap, filePath);
    }

    public void PopulateEnemyMap()
    {

        EnemyMap = CreateLayeredMap(dum.enemies, 10);
        
    }

    public void PopulateLootMap()
    {

        //LootMap = CreateLayeredMap(dum.itemContainers, 50);
    }

    public void PopulateNPCMap()
    {

        NpcMap = CreateLayeredMap(dum.npcs, 10);
        string filePath = @"C:\Users\bentg\Downloads\NPCmap_output.txt";
        PrintMapToFile(NpcMap, filePath);
    }

    public float GetPlayerMapValue(Vector2Int coord)
    {
        if (PlayerMap != null && PlayerMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public float GetEnemyMapValue(Vector2Int coord)
    {
        if (EnemyMap != null && EnemyMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public float GetNpcMapValue(Vector2Int coord)
    {
        if (NpcMap != null && NpcMap.TryGetValue(coord, out float value))
            return value;
        return float.MaxValue;
    }

    public float GetLootMapValue(Vector2Int coord)
    {
        if (LootMap != null && LootMap.TryGetValue(coord, out float value))
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
