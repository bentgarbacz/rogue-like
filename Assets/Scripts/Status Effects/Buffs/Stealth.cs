using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : StatusEffect
{
    
    private DungeonManager dum;

    public Stealth(CharacterSheet affectedCharacter, int duration, DungeonManager dum)
    {

        this.type = EffectType.Buff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.dum = dum;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Slink Away");
        this.isUnique = true;

        dum.enemiesOnLookout = false;
        dum.ClearAggroBuffer();
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void EndEffect()
    {
        
        dum.enemiesOnLookout = true;
    }

    public override string GetDescription()
    {

        return "Unseen by enemies";
    }
}
