using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Equipment
{

    public Amulet(int dropChance = 0 )
    {

        this.title = "Amulet";
        this.description = "Necklace imbued with magical properties.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Amulet");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.Amulet;

        this.bonusStatDictionary[StatType.CritChance] = 15;
        this.bonusStatDictionary[StatType.IntelligenceRequirement] = 5;

        SetDropChance(dropChance);
    }
}
