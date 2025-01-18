using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public GameObject hero;
    private PlayerCharacterSheet playerCharacter;
    private InventoryManager im;

    void Start()
    {
        
        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
    }

    public void UpdateStats()
    { 

        //Subtract stat bonuses from all stats
        playerCharacter.strength -= playerCharacter.strengthBonus;
        playerCharacter.dexterity -= playerCharacter.dexterityBonus;
        playerCharacter.intelligence -= playerCharacter.intelligenceBonus;
        playerCharacter.speed -= playerCharacter.speedBonus;
        playerCharacter.critChance -= playerCharacter.critChanceBonus;
        playerCharacter.armor -= playerCharacter.armorBonus;
        playerCharacter.evasion -= playerCharacter.evasionBonus;
        playerCharacter.accuracy -= playerCharacter.accuracyBonus;
        playerCharacter.minDamage -= playerCharacter.minDamageBonus;
        playerCharacter.maxDamage -= playerCharacter.maxDamageBonus;
        playerCharacter.maxHealth -= playerCharacter.maxHealthBonus;
        playerCharacter.maxMana -= playerCharacter.maxManaBonus;

        //Set all stat bonuses to zero
        playerCharacter.strengthBonus = 0;
        playerCharacter.dexterityBonus = 0;
        playerCharacter.intelligenceBonus = 0;
        playerCharacter.speedBonus = 0;
        playerCharacter.critChanceBonus = 0;
        playerCharacter.armorBonus = 0;
        playerCharacter.evasionBonus = 0;
        playerCharacter.accuracyBonus = 0;
        playerCharacter.minDamageBonus = 0;
        playerCharacter.maxDamageBonus = 0;
        playerCharacter.maxHealthBonus = 0;
        playerCharacter.maxManaBonus = 0;


        //For each equiped item, add stat bonuses provided by item to the overall stat bonus
        foreach(ItemSlot slot in im.equipmentSlotsDictionary.Values)
        {

            if(slot.item != null)
            {

                if(slot.item is Equipment equipment)
                {

                    playerCharacter.strengthBonus += equipment.bonusStatDictionary[StatType.Strength];
                    playerCharacter.dexterityBonus += equipment.bonusStatDictionary[StatType.Dexterity];
                    playerCharacter.intelligenceBonus += equipment.bonusStatDictionary[StatType.Intelligence];
                    playerCharacter.speedBonus += equipment.bonusStatDictionary[StatType.Speed];
                    playerCharacter.critChanceBonus += equipment.bonusStatDictionary[StatType.CritChance];
                    playerCharacter.armorBonus += equipment.bonusStatDictionary[StatType.Armor];
                    playerCharacter.evasionBonus += equipment.bonusStatDictionary[StatType.Evasion];
                    playerCharacter.accuracyBonus += equipment.bonusStatDictionary[StatType.Accuracy];
                    playerCharacter.minDamageBonus += equipment.bonusStatDictionary[StatType.MinDamage];
                    playerCharacter.maxDamageBonus += equipment.bonusStatDictionary[StatType.MaxDamage];
                    playerCharacter.maxHealthBonus += equipment.bonusStatDictionary[StatType.MaxHealth];
                    playerCharacter.maxManaBonus += equipment.bonusStatDictionary[StatType.MaxMana];
                }
            }
        }

        //Calculate secondary stat bonuses granted by strength
        playerCharacter.armorBonus += (playerCharacter.strengthBonus + playerCharacter.strength) / 10;
        playerCharacter.maxHealthBonus += (playerCharacter.strengthBonus + playerCharacter.strength) / 2;

        //Calculate secondary stat bonuses granted by dexterity
        playerCharacter.evasionBonus += (playerCharacter.dexterityBonus + playerCharacter.dexterity) * 5;
        playerCharacter.accuracyBonus += (playerCharacter.dexterityBonus + playerCharacter.dexterity) * 2;

        //Calculate secondary stat bonuses granted by intelligence
        playerCharacter.critChanceBonus += (playerCharacter.intelligenceBonus + playerCharacter.intelligence) / 5;
        playerCharacter.maxManaBonus += (playerCharacter.intelligenceBonus + playerCharacter.intelligence) / 2;

        //Add newly calculated stat bonuses to the actual stat
        playerCharacter.strength += playerCharacter.strengthBonus;
        playerCharacter.dexterity += playerCharacter.dexterityBonus;
        playerCharacter.intelligence += playerCharacter.intelligenceBonus;
        playerCharacter.speed += playerCharacter.speedBonus;
        playerCharacter.critChance += playerCharacter.critChanceBonus;
        playerCharacter.armor += playerCharacter.armorBonus;
        playerCharacter.evasion += playerCharacter.evasionBonus;
        playerCharacter.accuracy += playerCharacter.accuracyBonus;
        playerCharacter.minDamage += playerCharacter.minDamageBonus;
        playerCharacter.maxDamage += playerCharacter.maxDamageBonus;
        playerCharacter.maxHealth += playerCharacter.maxHealthBonus;
        playerCharacter.maxMana += playerCharacter.maxManaBonus;

        //Set health and mana values so that they do not exceed their maximum values after recalculating stats
        playerCharacter.health = Mathf.Min(playerCharacter.health, playerCharacter.maxHealth);
        playerCharacter.mana = Mathf.Min(playerCharacter.mana, playerCharacter.maxMana);
    } 

    //Checks to see if given equipment can be equiped based on its stat requirements
    public bool MeetsRequirements(Equipment equipment)
    {

        if(playerCharacter.strength >= equipment.bonusStatDictionary[StatType.StrengthRequirement] && 
           playerCharacter.dexterity >= equipment.bonusStatDictionary[StatType.DexterityRequirement] && 
           playerCharacter.intelligence >= equipment.bonusStatDictionary[StatType.IntelligenceRequirement])
        {

            return true;
        }

        return false;
    }

    //Checks to see if item in originSlot is permited to be placed in destinationSlot
    public bool ValidEquip(ItemSlot destinationSlot, ItemSlot originSlot)
    {
        
        if(Rules.CheckValidEquipmentSlot(destinationSlot, originSlot.item) && (Rules.CheckValidEquipmentSlot(originSlot, destinationSlot.item) ||
           destinationSlot.item == null))
        {

            if(im.equipmentSlotsDictionary.ContainsKey(destinationSlot.type) && originSlot.item is Equipment equipment)
            {

                return MeetsRequirements(equipment);            
            }
        }

        return false;
    }
}
