using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion1 : Consumable
{
    private GameObject hero = GameObject.Find("System Managers").GetComponent<DungeonManager>().hero;
    private readonly int manaRestoreValue = 15;

    public ManaPotion1(int dropChance = 0 )
    {

        this.sprite = Resources.Load<Sprite>("Pixel Art/Items/Mana Small Potion");
        this.title = "Small Mana Potion";    
        this.description = "Drink this to regain 15 mana.";    
        this.contextText = "Drink";
        this.contextClip = Resources.Load<AudioClip>("Sounds/Drink");
        SetDropChance(dropChance);
    }

    public override void Use()
    {

        hero.GetComponent<PlayerCharacterSheet>().RegainMana(manaRestoreValue);
    }
}
