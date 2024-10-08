using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScroll : Scroll
{

    public TeleportScroll(int dropChance = 0 )
    {

        this.intelligenceRequirement = 15;
        this.title = "Teleport Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.Teleport;
        SetDropChance(dropChance);
    }
}
