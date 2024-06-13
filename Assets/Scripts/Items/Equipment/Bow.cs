using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Bow(int dropChance = 0 )
    {

        this.title = "Bow";
        this.description = "Ranged weapon, fires arrows.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Bow");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Main Hand";

        this.bonusStatDictionary["Min Damage"] = 1;
        this.bonusStatDictionary["Max Damage"] = 3;
        this.bonusStatDictionary["Accuracy"] = 20;
        this.bonusStatDictionary["Range"] = 8;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
