using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Ring(int dropChance = 0 )
    {

        this.title = "Ring";
        this.description = "Golden loop imbued with magical properties.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Ring");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Ring";

        this.bonusStatDictionary["Accuracy"] = 100;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
