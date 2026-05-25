using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nimble : CharacterModifier
{
    private int evasionIncrease = 15;
    private int accuracyIncrease = 15;

    public Nimble(CharacterSheet affectedCharacter)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.affectedCharacter = affectedCharacter;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Swift");
    }

    public override void StartEffect()
    {
        affectedCharacter.stats.evasion += evasionIncrease;
        affectedCharacter.stats.accuracy += accuracyIncrease;
    }

    public override void EndEffect()
    {
        affectedCharacter.stats.evasion -= evasionIncrease;
        affectedCharacter.stats.accuracy -= accuracyIncrease;
    }

    public override string GetDescription()
    {
        return "Increased evasion and accuracy";
    }
}
