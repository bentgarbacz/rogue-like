using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Amulet(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Amulet");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Amulet";

        this.critChanceBonus = 15;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
