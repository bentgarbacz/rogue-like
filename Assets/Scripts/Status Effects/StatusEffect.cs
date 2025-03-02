using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{

    public EffectType type = EffectType.None;
    public int duration = 0;
    public CharacterSheet affectedCharacter;
    public Sprite sprite;

    public virtual int Effect()
    {

        return duration;
    }

    public virtual void EndEffect()
    {

        return;
    }

    public virtual string GetDescription()
    {

        return type.ToString();
    }
}
