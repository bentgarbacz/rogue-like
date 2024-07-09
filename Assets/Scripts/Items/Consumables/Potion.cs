using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{

    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;
    private readonly int healValue = 10;

    public Potion(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Items/Small Potion");
        this.title = "Small Potion";    
        this.description = "Drink this to regain 10 health.";    
        this.contextText = "Drink";
        this.contextClip = Resources.Load<AudioClip>("Sounds/Drink");
        SetDropChance(dropChance);
    }

    public override void Use()
    {

        hero.GetComponent<Character>().Heal(healValue);
    }
}
