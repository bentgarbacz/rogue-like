using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equipment
{

    public Shield(int dropChance = 0 )
    {

        this.title = "Wooden Shield";
        this.description = "Will deflect a blow in a pinch.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Shield");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Off Hand";

        this.bonusStatDictionary["Armor"] = 1;

        SetDropChance(dropChance);
    }
}
