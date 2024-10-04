using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScroll : Scroll
{

    public HealScroll(int dropChance = 0 )
    {

        this.intelligenceRequirement = 5;
        this.title = "Heal Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.Heal;
        SetDropChance(dropChance);
    }
}
