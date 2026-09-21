using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLeech : CharacterModifier
{

    public int lifeGainOnHit = 0;
    public float lifeSteal = 0f;

    public LifeLeech(CharacterSheet affectedCharacter, int lifeGainOnHit, float lifeSteal)
    {

        this.descriptors.Add(ModifierDescriptor.OneShot);
        this.descriptors.Add(ModifierDescriptor.Buff);
        this.descriptors.Add(ModifierDescriptor.NoVisual);

        this.affectedCharacter = affectedCharacter;
        this.lifeGainOnHit = lifeGainOnHit;
        this.lifeSteal = lifeSteal;
    }

    public override void StartEffect()
    {

        int healval = (int)(damageDealt * lifeSteal) + lifeGainOnHit;

        affectedCharacter.GetComponent<CharacterHealth>().Heal(healval);
    }

    public override string GetDescription()
    {
        return "Recover HP on attack";
    }
}