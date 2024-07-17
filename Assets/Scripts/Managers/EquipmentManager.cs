using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public GameObject hero;
    private PlayerCharacter playerCharacter;
    private InventoryManager im;

    void Start()
    {
        
        playerCharacter = hero.GetComponent<PlayerCharacter>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
    }

    public void UpdateStats()
    {

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


        foreach(ItemSlot slot in im.equipmentSlotsDictionary.Values)
        {

            if(slot.item != null)
            {

                if(slot.item is Equipment equipment)
                {

                    playerCharacter.strengthBonus += equipment.bonusStatDictionary["Strength"];
                    playerCharacter.dexterityBonus += equipment.bonusStatDictionary["Dexterity"];
                    playerCharacter.intelligenceBonus += equipment.bonusStatDictionary["Intelligence"];
                    playerCharacter.speedBonus += equipment.bonusStatDictionary["Speed"];
                    playerCharacter.critChanceBonus += equipment.bonusStatDictionary["Crit Chance"];
                    playerCharacter.armorBonus += equipment.bonusStatDictionary["Armor"];
                    playerCharacter.evasionBonus += equipment.bonusStatDictionary["Evasion"];
                    playerCharacter.accuracyBonus += equipment.bonusStatDictionary["Accuracy"];
                    playerCharacter.minDamageBonus += equipment.bonusStatDictionary["Min Damage"];
                    playerCharacter.maxDamageBonus += equipment.bonusStatDictionary["Max Damage"];
                    playerCharacter.maxHealthBonus += equipment.bonusStatDictionary["Max Health"];

                }
            }
        }

        playerCharacter.armorBonus += (playerCharacter.strengthBonus + playerCharacter.strength) / 10;
        playerCharacter.maxHealthBonus += (playerCharacter.strengthBonus + playerCharacter.strength) / 2;

        playerCharacter.evasionBonus += (playerCharacter.dexterityBonus + playerCharacter.dexterity) * 5;
        playerCharacter.speedBonus += playerCharacter.dexterityBonus + playerCharacter.dexterity;

        playerCharacter.critChanceBonus += (playerCharacter.intelligenceBonus + playerCharacter.intelligence) / 2;
        playerCharacter.accuracyBonus += (playerCharacter.intelligenceBonus + playerCharacter.intelligence) * 2;

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
    } 

    public bool MeetsRequirements(Equipment equipment)
    {

        if(playerCharacter.strength >= equipment.bonusStatDictionary["Strength Requirement"] && 
           playerCharacter.dexterity >= equipment.bonusStatDictionary["Dexterity Requirement"] && 
           playerCharacter.intelligence >= equipment.bonusStatDictionary["Intelligence Requirement"])
        {

            return true;
        }

        return false;
    }

    public bool ValidEquip(ItemSlot destinationSlot, ItemSlot originSlot)
    {

        if((Rules.CheckValidEquipmentSlot(destinationSlot, originSlot.item) && Rules.CheckValidEquipmentSlot(originSlot, destinationSlot.item)) ||
           (Rules.CheckValidEquipmentSlot(destinationSlot, originSlot.item) && destinationSlot.item == null))
        {

            if(im.equipmentSlotsDictionary.ContainsKey(destinationSlot.type) && originSlot.item is Equipment equipment)
            {

                return MeetsRequirements(equipment);            
            }
        }

        return false;
    }
}
