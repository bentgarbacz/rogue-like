using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Shield(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Shield");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Off Hand";

        this.armorBonus = 5;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
