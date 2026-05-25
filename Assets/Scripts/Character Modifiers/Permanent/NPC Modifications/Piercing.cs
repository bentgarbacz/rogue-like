using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : CharacterModifier
{
    private int armorPenetrationIncrease = 5;

    public Piercing(CharacterSheet affectedCharacter)
    {

        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.descriptors.Add(ModifierDescriptor.Buff);

        this.affectedCharacter = affectedCharacter;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Pierce");
    }

    public override void StartEffect()
    {

        affectedCharacter.stats.armorPenetration += armorPenetrationIncrease;
    }

    public override void EndEffect()
    {

        affectedCharacter.stats.armorPenetration -= armorPenetrationIncrease;
    }

    public override string GetDescription()
    {
        return "Increased armor penetration";
    }
}