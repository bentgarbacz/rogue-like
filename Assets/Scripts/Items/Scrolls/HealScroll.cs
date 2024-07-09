using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScroll : Scroll
{

    public HealScroll(int dropChance = 0 )
    {

        this.intelligenceRequirement = 3;
        this.title = "Heal Scroll";    
        this.description = "Read this incantation to cast a spell.";
        this.spellName = "Heal";
        SetDropChance(dropChance);
    }
}
