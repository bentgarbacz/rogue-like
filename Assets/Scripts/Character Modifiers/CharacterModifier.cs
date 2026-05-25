using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModifier
{

    public HashSet<ModifierDescriptor> descriptors = new();
    public CharacterSheet affectedCharacter;
    public int duration = 0;
    public float procChance = 0f;
    public Sprite sprite;

    public virtual int Effect()
    {

        return duration;
    }

    public virtual void StartEffect()
    {
        
        return;
    }

    public virtual void EndEffect()
    {

        return;
    }

    public virtual CharacterModifier GetCombatModifier(CharacterSheet targetCharacter)
    {
        
        return null;
    }

    public virtual string GetDescription()
    {

        return "NULL";
    }
}
