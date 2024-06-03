using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootTableManager
{
    public static Dictionary<string, List<Item>> lootTableLookup = SetLootTables();

    private static Dictionary<string, List<Item>> SetLootTables()
    {

        //Dictionary containing ledger of potential items that can spawn from various entities
        Dictionary<string, List<Item>> lootTableLookup = new Dictionary<string, List<Item>>();

        lootTableLookup.Add(
                                "Chest",
                                new List<Item>(){
                                    new Amulet(100),
                                    new Dagger(100),  
                                    new Dagger(100),
                                    new PlateBody(100),
                                    new Shield(100),
                                    new Ring(100),
                                    new LeatherHelm(100),
                                    new LeatherBoots(100)
                                }
        );

        lootTableLookup.Add(
                                "Goblin",
                                new List<Item>(){
                                    new Potion(50),
                                    new Dagger(50),
                                    new Meat(100)
                                }
        );

        lootTableLookup.Add(
                                "Skeleton",
                                new List<Item>(){
                                    new Potion(100),
                                    new Meat(50)
                                }
        );

        return lootTableLookup;
    }

    public static List<Item> CreateItems(string dropTable)
    {

        //Randomly generate items for a given entity
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
