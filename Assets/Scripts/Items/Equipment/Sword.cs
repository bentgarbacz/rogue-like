using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Sword(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Sword");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Main Hand";

        this.minDamageBonus = 4;
        this.maxDamageBonus = 10;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
