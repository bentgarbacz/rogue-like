using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robes : Equipment
{

    public Robes(int dropChance = 0 )
    {

        this.title = "Robes";
        this.description = "Magical body armor";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Robes");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.Chest;

        this.bonusStatDictionary[StatType.Evasion] = 10;
        this.bonusStatDictionary[StatType.MaxMana] = 10;

        SetDropChance(dropChance);
    }
}
