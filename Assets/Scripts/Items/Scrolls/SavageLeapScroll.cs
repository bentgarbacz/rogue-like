using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavageLeapScroll : Scroll
{

    public SavageLeapScroll(int dropChance = 0 )
    {

        this.strengthRequirement = 5;
        this.title = "Savage Leap Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.strengthRequirement + " strength";
        this.spellType = SpellType.SavageLeap;
        SetDropChance(dropChance);
    }
}
