using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Equipment
{

    public Dagger(int dropChance = 0 )
    {

        this.title = "Dagger";
        this.description = "Easily concealed one handed weapon.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Dagger");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.type = EquipmentType.SwitchHand;

        this.bonusStatDictionary[StatType.MinDamage] = 2;
        this.bonusStatDictionary[StatType.MaxDamage] = 5;
        this.bonusStatDictionary[StatType.Accuracy] = 100;

        SetDropChance(dropChance);
    }
}
