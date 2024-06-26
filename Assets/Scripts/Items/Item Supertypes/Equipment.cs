using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public string equipmentType;

    public Dictionary<string, int> bonusStatDictionary;

    public Equipment()
    {

        bonusStatDictionary = new Dictionary<string, int>
        {

            { "Strength", 0 },
            { "Dexterity", 0 },
            { "Intelligence", 0 },
            { "Speed", 0 },
            { "Crit Chance", 0 },
            { "Armor", 0 },
            { "Evasion", 0 },
            { "Accuracy", 0 },
            { "Min Damage", 0 },
            { "Max Damage", 0 },
            { "Max Health", 0 },
            { "Strength Requirement", 0 },
            { "Dexterity Requirement", 0 },
            { "Intelligence Requirement", 0 }
        };

        equipmentType = "N/A";
        contextText = "Equip";
    }
}
