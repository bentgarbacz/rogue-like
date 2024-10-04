using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousStrikeScroll : Scroll
{
    public PoisonousStrikeScroll(int dropChance = 0 )
    {

        this.dexterityRequirement = 10;
        this.title = "Poisonous Strike Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.dexterityRequirement + " dexterity";
        this.spellType = SpellType.PoisonousStrike;
        SetDropChance(dropChance);
    }
}
