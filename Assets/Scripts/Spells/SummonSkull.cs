using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkull : Spell
{
    
    public SummonSkull()
    {

        this.spellType = SpellType.SummonSkull;
        this.targeted = true;
        this.cooldown = 3;
        this.manaCost = 15;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/SkullMinion");
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        return true;
    }
}
