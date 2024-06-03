using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherBoots : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public LeatherBoots(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Boots");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Boot";

        this.speedBonus = 15;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
