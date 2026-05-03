using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResilienceSpellScroll : Scroll
{
    public ResilienceSpellScroll(int dropChance = 0)
    {
        this.intelligenceRequirement = 10;
        this.title = "Resilience Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.ResilienceSpell;
        SetDropChance(dropChance);
    }
}