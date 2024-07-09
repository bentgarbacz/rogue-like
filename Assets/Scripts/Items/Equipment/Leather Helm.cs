using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherHelm : Equipment
{

    public LeatherHelm(int dropChance = 0 )
    {

        this.title = "Leather Helmet";
        this.description = "Keeps your ears warm.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Leather Helm");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Helmet";

        this.bonusStatDictionary["Evasion"] = 50;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
