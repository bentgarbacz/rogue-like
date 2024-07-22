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

        this.bonusStatDictionary["Evasion"] = 10;
        this.bonusStatDictionary["Max Mana"] = 10;

        SetDropChance(dropChance);
    }
}
