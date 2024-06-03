using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Ring(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Ring");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Ring";

        this.accuracyBonus = 100;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
