using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : Consumable
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;
    private readonly int hungerValue = 333;

    public Meat(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Items/Meat");
        this.title = "Meat";    
        this.description = "Eat to reduce your hunger";    
        this.contextText = "Eat";
        this.contextClip = Resources.Load<AudioClip>("Sounds/Eat");
        SetDropChance(dropChance);
    }

    public override void Use()
    {

        hero.GetComponent<PlayerCharacter>().SatiateHunger(hungerValue);
    }
}