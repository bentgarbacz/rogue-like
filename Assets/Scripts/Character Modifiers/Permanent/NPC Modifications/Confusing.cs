using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusing : CharacterModifier
{
    public int teleportRadius = 4;

    public Confusing(CharacterSheet affectedCharacter, int teleportRadius = 3)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.descriptors.Add(ModifierDescriptor.OnHit);

        this.procChance = 0.3f;

        this.affectedCharacter = affectedCharacter;
        this.teleportRadius = teleportRadius;

        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/QuestionMark");
    }

    public override CharacterModifier GetCombatModifier(CharacterSheet targetCharacter)
    {
        return new TemporalDisplacement(targetCharacter, teleportRadius);
    }

    public override string GetDescription()
    {
        return "May relocate target on hit";
    }
}
