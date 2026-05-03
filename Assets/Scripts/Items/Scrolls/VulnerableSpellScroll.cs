using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableSpellScroll : Scroll
{
    public VulnerableSpellScroll(int dropChance = 0)
    {
        this.intelligenceRequirement = 10;
        this.title = "Vulnerable Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.VulnerableSpell;
        SetDropChance(dropChance);
    }
}