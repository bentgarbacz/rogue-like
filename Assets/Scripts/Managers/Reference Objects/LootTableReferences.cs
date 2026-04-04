using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LootTableReferences
{
    public static Dictionary<DropTableType, List<Item>> lootTableLookup = SetLootTables();

    private static Dictionary<DropTableType, List<Item>> SetLootTables()
    {

        //Dictionary containing ledger of potential items that can spawn from various entities
        Dictionary<DropTableType, List<Item>> lootTableLookup = new()
        {
            {
                DropTableType.DebugChest,
                new List<Item>(){
                                    new SummonSkullScroll(100),
                                    new LiftScroll(100),
                                    new ClairvoyanceScroll(100),
                                    new DebugHelm(100),
                                    new SlinkAwayScroll(100),
                                    new FortifyScroll(100),
                                    new SavageLeapScroll(100),
                                    new PoisonousStrikeScroll(100),
                                    new Robes(100),
                                    new ManaPotion1(100),
                                    new FireballScroll(100),
                                    new TeleportScroll(100),
                                    new Bow(100),
                                    new PlateBody(100),
                                    new Shield(100),
                                    new Ring(100),                                    
                                    new Dagger(100)
                                }
            },
            {
                DropTableType.Chest,
                new List<Item>(){   
                                    new LeatherBoots(10),
                                    new LeatherChest(10),
                                    new LeatherHelm(10),
                                    new Robes(10),
                                    new FireballScroll(10),
                                    new HealScroll(10),
                                    new TeleportScroll(10),
                                    new Bow(10),
                                    new PlateBody(10),
                                    new Shield(10),
                                    new Ring(10),
                                    new Amulet(10),
                                    new Dagger(10),
                                    new Sword(10)
                                }
            },
            {
                DropTableType.Goblin,
                new List<Item>(){
                                    new HealthPotion1(7),
                                    new ManaPotion1(7),
                                    new Meat(7)
                                }
            },
            {
                DropTableType.Skeleton,
                new List<Item>(){
                                    new HealthPotion1(15),
                                    new ManaPotion1(15)
                                }
            },
            {
                DropTableType.Rat,
                new List<Item>(){
                                    new Meat(33)
                                }
            },
            {
                DropTableType.Slime,
                new List<Item>(){
                                    new BlueJelly(33)
                                }
            },
            {
                DropTableType.Witch,
                new List<Item>(){
                                    new FireballScroll(10),
                                    new HealScroll(10),
                                    new TeleportScroll(10)
                                }
            }
        };

        return lootTableLookup;
    }

    public static List<Item> CreateItems(DropTableType dropTable)
    {

        //Randomly generate items for a given entity
        List<Item> droppedItems = new();

        if(!lootTableLookup.Keys.Contains(dropTable))
        {
            
            return droppedItems;
        }
        
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
