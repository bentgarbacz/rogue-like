using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robes : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Robes(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Robes");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Chest";

        this.armorBonus = 1;
        this.evasionBonus = 50;
        this.intelligenceBonus = 5;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
