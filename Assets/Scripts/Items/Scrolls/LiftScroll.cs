using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScroll : Scroll
{

    public LiftScroll(int dropChance = 0 )
    {

        this.intelligenceRequirement = 10;
        this.title = "Lift Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";    
        this.spellType = SpellType.Lift;
        SetDropChance(dropChance);
    }
}