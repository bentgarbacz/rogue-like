using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Dagger(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Dagger");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Switch Hand";

        this.minDamageBonus = 2;
        this.maxDamageBonus = 5;
        this.accuracyBonus = 100;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
