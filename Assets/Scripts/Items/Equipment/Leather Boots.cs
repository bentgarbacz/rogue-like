using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherBoots : Equipment
{

    public LeatherBoots(int dropChance = 0 )
    {

        this.title = "Boots";
        this.description = "Reliable leather boots, shows signs of many adventures.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Boots");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Boot";

        this.bonusStatDictionary["Speed"] = 15;

        SetDropChance(dropChance);
    }
}
