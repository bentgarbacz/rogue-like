using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortify : Spell
{

    private readonly int armorIncrease = 5;
    private readonly int duration = 25;

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

        CharacterSheet casterCharacter = caster.GetComponent<CharacterSheet>();
        StatusEffectManager statusEffectManager = caster.GetComponent<StatusEffectManager>();

        caster.GetComponent<StatusEffectManager>().AddEffect(new StoneSkin(casterCharacter, duration, armorIncrease));
        ResetCooldown(caster);

        if(casterCharacter is PlayerCharacterSheet pc)
        {

            pc.UpdateUI();
        }

        return true;
    }
}
