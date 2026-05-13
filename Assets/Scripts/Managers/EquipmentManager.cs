using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{

    public GameObject hero;
    private PlayerCharacterSheet playerCharacter;
    private CharacterStats stats;
    private PlayerHealth playerHealth;
    private InventoryManager im;

    void Start()
    {
        
        playerCharacter = hero.GetComponent<PlayerCharacterSheet>();
        stats = playerCharacter.stats;
        playerHealth = hero.GetComponent<PlayerHealth>();
        im = GameObject.Find("System Managers").GetComponent<InventoryManager>();
    }

    public void UpdateStats()
    { 
        //End all active status effects to remove their stat modifications
        List<StatusEffect> activeStatusEffects = playerCharacter.statusEffectMgr.GetStatusEffects();
        foreach(StatusEffect effect in activeStatusEffects)
        {
            effect.EndEffect();
        }

        //Subtract stat bonuses from all stats
        stats.strength -= playerCharacter.strengthBonus;
        stats.dexterity -= playerCharacter.dexterityBonus;
        stats.intelligence -= playerCharacter.intelligenceBonus;
        stats.speed -= playerCharacter.speedBonus;
        stats.critChance -= playerCharacter.critChanceBonus;
        stats.critMultiplier -= playerCharacter.critMultiplierBonus;
        stats.armor -= playerCharacter.armorBonus;
        stats.armorPenetration -= playerCharacter.armorPenetrationBonus;
        stats.evasion -= playerCharacter.evasionBonus;
        stats.accuracy -= playerCharacter.accuracyBonus;
        stats.minDamage -= playerCharacter.minDamageBonus;
        stats.maxDamage -= playerCharacter.maxDamageBonus;
        stats.maxHealth -= playerCharacter.maxHealthBonus;
        stats.maxMana -= playerCharacter.maxManaBonus;
        stats.maxBarrier -= playerCharacter.maxBarrierBonus;

        //Set all stat bonuses to zero
        playerCharacter.strengthBonus = 0;
        playerCharacter.dexterityBonus = 0;
        playerCharacter.intelligenceBonus = 0;
        playerCharacter.speedBonus = 0;
        playerCharacter.critChanceBonus = 0;
        playerCharacter.critMultiplierBonus = 0;
        playerCharacter.armorBonus = 0;
        playerCharacter.armorPenetrationBonus = 0;
        playerCharacter.evasionBonus = 0;
        playerCharacter.accuracyBonus = 0;
        playerCharacter.minDamageBonus = 0;
        playerCharacter.maxDamageBonus = 0;
        playerCharacter.maxHealthBonus = 0;
        playerCharacter.maxManaBonus = 0;
        playerCharacter.maxBarrierBonus = 0;
        playerCharacter.stats.maxBarrier = 0;

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
                    playerCharacter.critChanceBonus += equipment.bonusStatDictionary[StatType.CritMultiplier];
                    playerCharacter.armorBonus += equipment.bonusStatDictionary[StatType.Armor];
                    playerCharacter.armorPenetrationBonus += equipment.bonusStatDictionary[StatType.ArmorPenetration];
                    playerCharacter.evasionBonus += equipment.bonusStatDictionary[StatType.Evasion];
                    playerCharacter.accuracyBonus += equipment.bonusStatDictionary[StatType.Accuracy];
                    playerCharacter.minDamageBonus += equipment.bonusStatDictionary[StatType.MinDamage];
                    playerCharacter.maxDamageBonus += equipment.bonusStatDictionary[StatType.MaxDamage];
                    playerCharacter.maxHealthBonus += equipment.bonusStatDictionary[StatType.MaxHealth];
                    playerCharacter.maxManaBonus += equipment.bonusStatDictionary[StatType.MaxMana];
                    playerCharacter.maxBarrierBonus += equipment.bonusStatDictionary[StatType.MaxBarrier];
                }
            }
        }

        //Calculate secondary stat bonuses granted by strength
        playerCharacter.armorBonus += (playerCharacter.strengthBonus + playerCharacter.stats.strength) / 10;
        playerCharacter.maxHealthBonus += (playerCharacter.strengthBonus + playerCharacter.stats.strength) / 2;

        //Calculate secondary stat bonuses granted by dexterity
        playerCharacter.evasionBonus += (playerCharacter.dexterityBonus + playerCharacter.stats.dexterity) * 5;
        playerCharacter.accuracyBonus += (playerCharacter.dexterityBonus + playerCharacter.stats.dexterity) * 2;

        //Calculate secondary stat bonuses granted by intelligence
        playerCharacter.critChanceBonus += (playerCharacter.intelligenceBonus + playerCharacter.stats.intelligence) / 5;
        playerCharacter.maxManaBonus += (playerCharacter.intelligenceBonus + playerCharacter.stats.intelligence) / 2;

        //Add newly calculated stat bonuses to the actual stat
        stats.strength += playerCharacter.strengthBonus;
        stats.dexterity += playerCharacter.dexterityBonus;
        stats.intelligence += playerCharacter.intelligenceBonus;
        stats.speed += playerCharacter.speedBonus;
        stats.critChance += playerCharacter.critChanceBonus;
        stats.critMultiplier += playerCharacter.critMultiplierBonus;
        stats.armor += playerCharacter.armorBonus;
        stats.armorPenetration = playerCharacter.armorPenetrationBonus;
        stats.evasion += playerCharacter.evasionBonus;
        stats.accuracy += playerCharacter.accuracyBonus;
        stats.minDamage += playerCharacter.minDamageBonus;
        stats.maxDamage += playerCharacter.maxDamageBonus;
        stats.maxHealth += playerCharacter.maxHealthBonus;
        stats.maxMana += playerCharacter.maxManaBonus;
        stats.maxBarrier += playerCharacter.maxBarrierBonus;

        //Set health and mana values so that they do not exceed their maximum values after recalculating stats
        playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth, playerCharacter.stats.maxHealth);
        playerHealth.currentBarrier = Mathf.Min(playerHealth.currentBarrier, playerCharacter.stats.maxBarrier);
        playerCharacter.mana = Mathf.Min(playerCharacter.mana, stats.maxMana);

        //Reapply all active status effects after stat recalculation
        foreach(StatusEffect effect in activeStatusEffects)
        {
            effect.StartEffect();
        }

        playerCharacter.UpdateUI();
    } 

    //Checks to see if given equipment can be equiped based on its stat requirements
    public bool MeetsRequirements(Equipment equipment)
    {

        if(stats.strength >= equipment.bonusStatDictionary[StatType.StrengthRequirement] && 
           stats.dexterity >= equipment.bonusStatDictionary[StatType.DexterityRequirement] && 
           stats.intelligence >= equipment.bonusStatDictionary[StatType.IntelligenceRequirement])
        {

            return true;
        }

        return false;
    }

    //Checks to see if item in originSlot is permited to be placed in destinationSlot
    public bool ValidEquip(ItemSlot destinationSlot, ItemSlot originSlot)
    {
        
        if(GameFunctions.CheckValidEquipmentSlot(destinationSlot, originSlot.item) && (GameFunctions.CheckValidEquipmentSlot(originSlot, destinationSlot.item) ||
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
