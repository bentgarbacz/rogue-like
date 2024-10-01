using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkin : StatusEffect
{

    private int armorIncrease;

    public StoneSkin(CharacterSheet affectedCharacter, int duration, int armorIncrease)
    {

        this.type = "Buff";
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.armorIncrease = armorIncrease;
        
        affectedCharacter.armor += armorIncrease;
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void EndEffect()
    {

        affectedCharacter.armor -= armorIncrease;
    }
}
