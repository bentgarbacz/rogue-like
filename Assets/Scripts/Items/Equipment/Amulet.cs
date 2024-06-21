using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amulet : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Amulet(int dropChance = 0 )
    {

        this.title = "Amulet";
        this.description = "Necklace imbued with magical properties.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Amulet");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Amulet";

        this.bonusStatDictionary["Crit Chance"] = 15;
        this.bonusStatDictionary["Intelligence Requirement"] = 1;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
