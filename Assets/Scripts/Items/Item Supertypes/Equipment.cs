using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public string equipmentType;
    public Dictionary<string, int> bonusStatDictionary;

    public Equipment()
    {

        bonusStatDictionary = new Dictionary<string, int>();

        bonusStatDictionary.Add("Strength", 0);
        bonusStatDictionary.Add("Dexterity", 0);
        bonusStatDictionary.Add("Intelligence", 0);
        bonusStatDictionary.Add("Speed", 0);
        bonusStatDictionary.Add("Crit Chance", 0);
        bonusStatDictionary.Add("Armor", 0);
        bonusStatDictionary.Add("Evasion", 0);
        bonusStatDictionary.Add("Accuracy", 0);
        bonusStatDictionary.Add("Min Damage", 0);
        bonusStatDictionary.Add("Max Damage", 0);
        bonusStatDictionary.Add("Max Health", 0);        

        equipmentType = "N/A";
        contextText = "Equip";
    }

}
