using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResilienceSpell : Spell
{
    private readonly int duration = 5;
    private readonly float damageMultiplierTaken = 0.3f;

    public ResilienceSpell()
    {
        this.spellType = SpellType.ResilienceSpell;
        this.targeted = true;
        this.cooldown = 20;
        this.manaCost = 15;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/BlueUp");
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

        statusEffectManager.AddEffect(new Resilience(targetCharacter, duration, damageMultiplierTaken));
        ResetCooldown(caster);

        return true;
    }
}