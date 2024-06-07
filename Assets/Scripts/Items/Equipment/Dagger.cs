using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Equipment
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;

    public Dagger(int dropChance = 0 )
    {

        this.title = "Dagger";
        this.description = "Easily concealed one handed weapon.";

        this.sprite = Resources.Load<Sprite>("Pixel Art/Equipment/Dagger");
        this.contextClip = Resources.Load<AudioClip>("Sounds/Equip");

        this.equipmentType = "Switch Hand";

        this.bonusStatDictionary["Min Damage"] = 2;
        this.bonusStatDictionary["Max Damage"] = 5;
        this.bonusStatDictionary["Accuracy"] = 100;

        SetDropChance(dropChance);
    }

    public override void Use()
    {

    }
}
