using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{

    public string equipmentType;
    public int strengthBonus = 0;
    public int dexterityBonus = 0;
    public int intelligenceBonus = 0;
    public int speedBonus = 0;
    public int critChanceBonus = 0;
    public int armorBonus = 0;
    public int evasionBonus = 0;
    public int accuracyBonus = 0;
    public int minDamageBonus = 0;
    public int maxDamageBonus = 0;

    public Equipment()
    {

        equipmentType = "N/A";
        contextText = "Equip";
    }

}
