using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precise : CharacterModifier
{
    private int critIncrease = 20;
    private int speedIncrease;
    private float speedMultiplier = 0.5f;

    public Precise(CharacterSheet affectedCharacter)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.affectedCharacter = affectedCharacter;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Arrow");

        speedIncrease = (int)(affectedCharacter.stats.speed * speedMultiplier);
    }

    public override void StartEffect()
    {
        affectedCharacter.stats.critChance += critIncrease;
        affectedCharacter.stats.speed += speedIncrease;
    }

    public override void EndEffect()
    {
        affectedCharacter.stats.critChance -= critIncrease;
        affectedCharacter.stats.speed -= speedIncrease;
    }

    public override string GetDescription()
    {
        return "Increased speed and chance to crit";
    }
}
