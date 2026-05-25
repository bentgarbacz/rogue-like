using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkin : CharacterModifier
{

    private int armorIncrease;

    public StoneSkin(CharacterSheet affectedCharacter, int duration, int armorIncrease)
    {

        this.descriptors.Add(ModifierDescriptor.Buff);
        this.descriptors.Add(ModifierDescriptor.Temporary);
        
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.armorIncrease = armorIncrease;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Fortify");
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void StartEffect()
    {
        
        affectedCharacter.stats.armor += armorIncrease;
    }

    public override void EndEffect()
    {

        affectedCharacter.stats.armor -= armorIncrease;
    }

    public override string GetDescription()
    {

        return "Increase armor by " + armorIncrease.ToString();
    }
}
