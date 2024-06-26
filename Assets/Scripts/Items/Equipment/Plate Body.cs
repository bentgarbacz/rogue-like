using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBody : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public PlateBody(int dropChance = 0 )
    {

        this.title = "Plate Body Armor";
        this.description = "Sturdy as it is heavy.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Plate Body");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Chest";

        this.bonusStatDictionary["Armor"] = 5;
        this.bonusStatDictionary["Strength Requirement"] = 1;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
