using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public GameObject hero;
    private PlayerCharacter playerCharacter;

    void Start()
    {
        
        playerCharacter = hero.GetComponent<PlayerCharacter>();
    }

    public void UpdateStats(Dictionary<string, ItemSlot> equipmentSlotsDictionary)
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


        foreach(ItemSlot slot in equipmentSlotsDictionary.Values)
        {

            if(slot.item != null)
            {

                if(slot.item is Equipment equipment)
                {

                    playerCharacter.strengthBonus += equipment.strengthBonus;
                    playerCharacter.dexterityBonus += equipment.dexterityBonus;
                    playerCharacter.intelligenceBonus += equipment.intelligenceBonus;
                    playerCharacter.speedBonus += equipment.speedBonus;
                    playerCharacter.critChanceBonus += equipment.critChanceBonus;
                    playerCharacter.armorBonus += equipment.armorBonus;
                    playerCharacter.evasionBonus += equipment.evasionBonus;
                    playerCharacter.accuracyBonus += equipment.accuracyBonus;
                    playerCharacter.minDamageBonus += equipment.minDamageBonus;
                    playerCharacter.maxDamageBonus += equipment.maxDamageBonus;
                    playerCharacter.maxHealthBonus += equipment.maxHealthBonus;

                }
            }
        }

        playerCharacter.armorBonus += (playerCharacter.strengthBonus + playerCharacter.strength) / 10;
        playerCharacter.maxHealthBonus += (playerCharacter.strengthBonus + playerCharacter.strength) / 2;

        playerCharacter.evasionBonus += (playerCharacter.dexterityBonus + playerCharacter.dexterity) * 5;
        playerCharacter.speedBonus += (playerCharacter.dexterityBonus + playerCharacter.dexterity);

        playerCharacter.critChance += (playerCharacter.intelligenceBonus + playerCharacter.intelligence) / 2;
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
    } 
}
