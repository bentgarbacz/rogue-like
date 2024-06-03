using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherHelm : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public LeatherHelm(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Helm");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Helmet";

        this.evasionBonus = 50;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
