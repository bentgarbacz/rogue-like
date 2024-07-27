using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortifyScroll : Scroll
{

    public FortifyScroll(int dropChance = 0 )
    {

        this.strengthRequirement = 5;
        this.title = "Fortify Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.strengthRequirement + " strength";
        this.spellName = "Fortify";
        SetDropChance(dropChance);
    }
}
