using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherChest : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public LeatherChest(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Chest");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Chest";

        this.evasionBonus = 100;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
