using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stout : CharacterModifier
{
    private int healthIncrease;
    private float healthMultiplier = 0.33f;

    public Stout(CharacterSheet affectedCharacter)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.affectedCharacter = affectedCharacter;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Dwarf");

        healthIncrease = (int)(affectedCharacter.stats.maxHealth * healthMultiplier);
    }

    public override void StartEffect()
    {
        affectedCharacter.stats.maxHealth += healthIncrease;
        affectedCharacter.characterHealth.Heal(healthIncrease);
    }

    public override void EndEffect()
    {
        affectedCharacter.stats.maxHealth -= healthIncrease;
    }

    public override string GetDescription()
    {
        return "Increased maximum health";
    }
}
