using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poisonous : CharacterModifier
{

    public int poisonDuration = 10;

    public Poisonous(CharacterSheet affectedCharacter)
    {
        this.descriptors.Add(ModifierDescriptor.Permanent);
        this.descriptors.Add(ModifierDescriptor.OnHit);

        this.procChance = 0.3f;

        this.affectedCharacter = affectedCharacter;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Poisonous");
    }

    public override CharacterModifier GetCombatModifier(CharacterSheet targetCharacter)
    {

        int poisonDamage = Mathf.Max(affectedCharacter.stats.minDamage / 2, 1);

        return new Poison(targetCharacter, poisonDuration, poisonDamage);
    }

    public override string GetDescription()
    {
        return "May poison target on hit";
    }
}
