using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Sword(int dropChance = 0 )
    {

        this.title = "Sword";
        this.description = "It ain't pretty but it is pointy.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Sword");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Main Hand";

        this.bonusStatDictionary["Min Damage"] = 4;
        this.bonusStatDictionary["Max Damage"] = 10;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
