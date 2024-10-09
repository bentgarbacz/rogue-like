using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Equipment
{
    
    public Sword(int dropChance = 0 )
    {

        this.title = "Sword";
        this.description = "It ain't pretty but it is pointy.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Sword");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.MainHand;

        this.bonusStatDictionary[StatType.MinDamage] = 4;
        this.bonusStatDictionary[StatType.MaxDamage] = 10;
        this.bonusStatDictionary[StatType.StrengthRequirement] = 5;
        this.bonusStatDictionary[StatType.DexterityRequirement] = 5;

        SetDropChance(dropChance);
    }
}
