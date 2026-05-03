using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnrageSpell : Spell
{
    private readonly int duration = 5;
    private readonly float damageMultiplier = 0.5f;

    public EnrageSpell()
    {
        this.spellType = SpellType.EnrageSpell;
        this.targeted = true;
        this.cooldown = 20;
        this.manaCost = 15;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/RedUp");
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

        statusEffectManager.AddEffect(new Enrage(targetCharacter, duration, damageMultiplier));
        ResetCooldown(caster);

        return true;
    }
}