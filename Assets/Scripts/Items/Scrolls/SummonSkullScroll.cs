using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkullScroll : Scroll
{
    
    public SummonSkullScroll(int dropChance = 0)
    {

        this.intelligenceRequirement = 15;
        this.title = "Summon Skull Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.SummonSkull;
        SetDropChance(dropChance);
    }
}
