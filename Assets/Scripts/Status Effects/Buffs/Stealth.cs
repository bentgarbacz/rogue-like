using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : StatusEffect
{
    
    private EntityManager entityMgr;

    public Stealth(CharacterSheet affectedCharacter, int duration, EntityManager entityMgr)
    {

        this.type = EffectType.Buff;
        this.affectedCharacter = affectedCharacter;
        this.duration = duration;
        this.entityMgr = entityMgr;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Slink Away");
        this.isUnique = true;

        entityMgr.enemiesOnLookout = false;
        entityMgr.ClearAggroBuffer();
    }

    public override int Effect()
    {

        duration -= 1;
        return duration;
    }

    public override void EndEffect()
    {
        
        entityMgr.enemiesOnLookout = true;
    }

    public override string GetDescription()
    {

        return "Unseen by enemies";
    }
}
