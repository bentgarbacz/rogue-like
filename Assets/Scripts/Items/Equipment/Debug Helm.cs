using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelm : Equipment
{
    public DebugHelm(int dropChance = 0 )
    {

        this.title = "Debug Helmet";
        this.description = "wtf are you even doing.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Helm");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.Helmet;

        this.bonusStatDictionary[StatType.Evasion] = 10;
        this.bonusStatDictionary[StatType.Strength] = 99;
        this.bonusStatDictionary[StatType.Dexterity] = 99;
        this.bonusStatDictionary[StatType.Intelligence] = 99;

        SetDropChance(dropChance);
    }
}
