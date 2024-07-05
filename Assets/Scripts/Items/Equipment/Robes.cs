using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robes : Equipment
{

    public Robes(int dropChance = 0 )
    {

        this.title = "Robes";
        this.description = "The wisdom of it's previous owner rubs off on you.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Robes");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Chest";

        this.bonusStatDictionary["Armor"] = 1;
        this.bonusStatDictionary["Evasion"] = 50;
        this.bonusStatDictionary["Intelligence"] = 5;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
