using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisScroll : Scroll
{
    
    public TelekinesisScroll(int dropChance = 0)
    {

        this.intelligenceRequirement = 15;
        this.title = "Telekinesis Scroll";
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";
        this.spellType = SpellType.Telekinesis;
        SetDropChance(dropChance);
    }
}
