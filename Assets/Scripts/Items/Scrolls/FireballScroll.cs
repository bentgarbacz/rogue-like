using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScroll : Scroll
{

    public FireballScroll(int dropChance = 0 )
    {

        this.intelligenceRequirement = 10;
        this.title = "Fireball Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";    
        this.spellType = SpellType.Fireball;
        SetDropChance(dropChance);
    }
}
