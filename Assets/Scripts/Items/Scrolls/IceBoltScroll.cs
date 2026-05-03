using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBoltScroll : Scroll
{
    public IceBoltScroll(int dropChance = 0)
    {
        this.intelligenceRequirement = 10;
        this.title = "Ice Bolt Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.IceBolt;
        SetDropChance(dropChance);
    }
}
