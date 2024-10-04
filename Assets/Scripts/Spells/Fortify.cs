using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortify : Spell
{

    private readonly int armorIncrease = 5;
    private readonly int duration = 10;

    public Fortify()
    {

        this.spellType =SpellType.Fireball;
        this.targeted = false;
        this.cooldown = 30;
        this.manaCost = 20;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Fortify");
        this.castSound = Resources.Load<AudioClip>("Sounds/Fortify");
    }

    public override bool Cast(GameObject caster, GameObject target = null)
    {

        caster.GetComponent<StatusEffectManager>().statusEffects.Add(new StoneSkin(caster.GetComponent<CharacterSheet>(), duration, armorIncrease));
        ResetCooldown(caster);
        return true;
    }
}
