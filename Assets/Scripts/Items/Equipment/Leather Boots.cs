using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherBoots : Equipment
{

    public LeatherBoots(int dropChance = 0 )
    {

        this.title = "Boots";
        this.description = "Leather boots";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Boots");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.Boot;

        this.bonusStatDictionary[StatType.Speed] = 15;

        SetDropChance(dropChance);
    }
}
