using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LootTableManager
{
    public static Dictionary<string, List<Item>> lootTableLookup = SetLootTables();

    private static Dictionary<string, List<Item>> SetLootTables()
    {

        //Dictionary containing ledger of potential items that can spawn from various entities
        Dictionary<string, List<Item>> lootTableLookup = new()
        {
            {
                "DebugChest",
                new List<Item>(){   
                                    new ManaPotion1(100),
                                    new FireballScroll(100),
                                    new TeleportScroll(100),
                                    new HealScroll(100),
                                    new Bow(100),
                                    new PlateBody(100),
                                    new Shield(100),
                                    new Ring(100),
                                    new Dagger(100),
                                    new Dagger(100)
                                }
            },
            {
                "Chest",
                new List<Item>(){   
                                    new LeatherBoots(10),
                                    new LeatherChest(10),
                                    new LeatherHelm(10),
                                    new Robes(10),
                                    new FireballScroll(10),
                                    new HealScroll(10),
                                    new Bow(10),
                                    new PlateBody(10),
                                    new Shield(10),
                                    new Ring(10),
                                    new Amulet(10),
                                    new Dagger(10)
                                }
            },
            {
                "Goblin",
                new List<Item>(){
                                    new HealthPotion1(7),
                                    new ManaPotion1(7),
                                    new Meat(7)
                                }
            },
            {
                "Skeleton",
                new List<Item>(){
                                    new HealthPotion1(15),
                                    new ManaPotion1(15),
                                }
            },
            {
                "Rat",
                new List<Item>(){
                                    new Meat(50)
                                }
            }
        };

        return lootTableLookup;
    }

    public static List<Item> CreateItems(string dropTable)
    {

        //Randomly generate items for a given entity
        List<Item> droppedItems = new();

        if(lootTableLookup.Keys.Contains(dropTable))
        {
        
            foreach(Item i in lootTableLookup[dropTable])
            {

                int rng = UnityEngine.Random.Range(0, 100);

                if(rng <= i.dropChance)
                {

                    droppedItems.Add(i);
                }
            }
        }

        return droppedItems;
    }
}
