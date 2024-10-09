using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public EquipmentType type;

    public Dictionary<StatType, int> bonusStatDictionary;

    public Equipment()
    {

        bonusStatDictionary = new Dictionary<StatType, int>
        {

            { StatType.Strength, 0 },
            { StatType.Dexterity, 0 },
            { StatType.Intelligence, 0 },
            { StatType.Speed, 0 },
            { StatType.CritChance, 0 },
            { StatType.Armor, 0 },
            { StatType.Evasion, 0 },
            { StatType.Accuracy, 0 },
            { StatType.MinDamage, 0 },
            { StatType.MaxDamage, 0 },
            { StatType.MaxHealth, 0 },
            { StatType.MaxMana, 0 },
            { StatType.StrengthRequirement, 0 },
            { StatType.DexterityRequirement, 0 },
            { StatType.IntelligenceRequirement, 0 }
        };

        type = EquipmentType.None;
        contextText = "Equip";
    }
}
