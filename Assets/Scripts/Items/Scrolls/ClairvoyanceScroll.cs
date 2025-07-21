using UnityEngine;

public class ClairvoyanceScroll : Scroll
{

    public ClairvoyanceScroll(int dropChance = 0 )
    {

        this.intelligenceRequirement = 30;
        this.title = "Clairvoyance Scroll";    
        this.description = "Read this incantation to cast a spell.\nMemorization requirement: " + this.intelligenceRequirement + " intelligence";    
        this.spellType = SpellType.Clairvoyance;
        SetDropChance(dropChance);
    }
}
