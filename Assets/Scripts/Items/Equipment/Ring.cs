using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : Equipment
{

    public Ring(int dropChance = 0 )
    {

        this.title = "Ring";
        this.description = "Golden loop imbued with magical properties.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Ring");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.Ring;

        this.bonusStatDictionary[StatType.Accuracy] = 100;

        SetDropChance(dropChance);
    }
}
