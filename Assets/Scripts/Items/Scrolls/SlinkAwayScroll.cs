using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkAwayScroll : Scroll
{

    public SlinkAwayScroll(int dropChance = 0 )
    {

        this.dexterityRequirement = 5;
        this.title = "Slink Away Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.dexterityRequirement + " dexterity";
        this.spellType = SpellType.SlinkAway;
        SetDropChance(dropChance);
    }
}
