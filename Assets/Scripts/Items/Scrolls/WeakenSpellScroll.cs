using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakenSpellScroll : Scroll
{
    public WeakenSpellScroll(int dropChance = 0)
    {
        this.intelligenceRequirement = 10;
        this.title = "Weaken Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.WeakenSpell;
        SetDropChance(dropChance);
    }
}