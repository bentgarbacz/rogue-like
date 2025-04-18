using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherChest : Equipment
{

    public LeatherChest(int dropChance = 0 )
    {

        this.title = "Leather Body Armor";
        this.description = "Leather armor that protects your torso.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Chest");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.Chest;

        this.bonusStatDictionary[StatType.Evasion] = 20;

        SetDropChance(dropChance);
    }
}
