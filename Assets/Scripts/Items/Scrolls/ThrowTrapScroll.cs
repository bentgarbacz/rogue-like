using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTrapScroll : Scroll
{
    public ThrowTrapScroll(int dropChance = 0)
    {
        this.dexterityRequirement = 10;
        this.title = "Create Trap Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.dexterityRequirement + " dexterity";
        this.spellType = SpellType.ThrowTrap;
        SetDropChance(dropChance);
    }
}
