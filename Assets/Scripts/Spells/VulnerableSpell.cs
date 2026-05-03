using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableSpell : Spell
{
    private readonly int duration = 5;
    private readonly float damageMultiplierTaken = 0.3f;

    public VulnerableSpell()
    {
        this.spellType = SpellType.VulnerableSpell;
        this.targeted = true;
        this.cooldown = 20;
        this.manaCost = 15;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/BlueDown");
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

        statusEffectManager.AddEffect(new Vulnerable(targetCharacter, duration, damageMultiplierTaken));
        ResetCooldown(caster);

        return true;
    }
}