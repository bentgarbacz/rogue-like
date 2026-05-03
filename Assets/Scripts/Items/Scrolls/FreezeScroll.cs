using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeScroll : Scroll
{

    public FreezeScroll(int dropChance = 0)
    {

        this.intelligenceRequirement = 10;
        this.title = "Freeze Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.Freeze;
        SetDropChance(dropChance);
    }
}
