using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Spell
{

    private readonly int duration = 11;

    public Freeze()
    {

        this.spellType = SpellType.Freeze;
        this.targeted = true;
        this.cooldown = 15;
        this.manaCost = 15;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Frozen");
        this.castSound = Resources.Load<AudioClip>("Sounds/Fortify");
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(!target.GetComponent<CharacterSheet>())
        {

            return false;
        }

        CharacterSheet targetCharacter = target.GetComponent<CharacterSheet>();
        StatusEffectManager statusEffectManager = target.GetComponent<StatusEffectManager>();

        statusEffectManager.AddEffect(new Frozen(targetCharacter, duration));
        ResetCooldown(caster);

        return true;
    }
}
