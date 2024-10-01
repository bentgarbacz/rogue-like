using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{

    public string type = "None";
    public int duration = 0;
    public CharacterSheet affectedCharacter;

    public virtual int Effect()
    {

        return duration;
    }

    public virtual void EndEffect()
    {

        return;
    }
}
