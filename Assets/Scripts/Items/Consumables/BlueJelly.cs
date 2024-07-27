using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueJelly : Consumable
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;
    private readonly int hungerValue = 200;
    private readonly int manaRestoreValue = 5;

    public BlueJelly(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Items/Blue Jelly");
        this.title = "Blue Jelly";    
        this.description = "Eat to reduce your hunger and restore 5 mana";    
        this.contextText = "Eat";
        this.contextClip = Resources.Load<AudioClip>("Sounds/Eat");
        SetDropChance(dropChance);
    }

    public override void Use()
    {

        hero.GetComponent<PlayerCharacter>().SatiateHunger(hungerValue);
        hero.GetComponent<PlayerCharacter>().RegainMana(manaRestoreValue);
    }
}
