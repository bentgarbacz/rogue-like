using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitate : StatusEffect
{

    public Levitate(CharacterSheet affectedCharacter, int duration)
    {

        this.type = EffectType.Buff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;

        this.sprite = Resources.Load<Sprite>("Pixel Art/Wing");

        affectedCharacter.gameObject.GetComponent<Levitating>().StartLevitating();
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void EndEffect()
    {

        affectedCharacter.gameObject.GetComponent<Levitating>().EndLevitating();
    }

    public override string GetDescription()
    {

        return "Float in the air";
    }
}
