using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnrageSpellScroll : Scroll
{
    public EnrageSpellScroll(int dropChance = 0)
    {
        this.strengthRequirement = 10;
        this.title = "Enrage Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.strengthRequirement + " strength";
        this.spellType = SpellType.EnrageSpell;
        SetDropChance(dropChance);
    }
}