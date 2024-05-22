using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootTableManager
{
    public static Dictionary<string, List<Item>> lootTableLookup = SetLootTables();

    private static Dictionary<string, List<Item>> SetLootTables()
    {
        Dictionary<string, List<Item>> lootTableLookup = new Dictionary<string, List<Item>>();

        lootTableLookup.Add(
                                "Chest",
                                new List<Item>(){
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(50)
                                }
        );

        lootTableLookup.Add(
                                "Goblin",
                                new List<Item>(){
                                    new Potion(100),
                                    new Potion(50)
                                }
        );

        lootTableLookup.Add(
                                "Skeleton",
                                new List<Item>(){
                                    new Potion(100),
                                    new Potion(100),
                                    new Potion(50)
                                }
        );

        return lootTableLookup;
    }
    public static List<Item> CreateItems(string dropTable)
    {

        List<Item> droppedItems = new List<Item>();

        foreach(Item i in lootTableLookup[dropTable])
        {

            int rng = UnityEngine.Random.Range(0, 100);

            if(rng <= i.dropChance)
            {

                droppedItems.Add(i);
            }
        }

        return droppedItems;
    }
}
