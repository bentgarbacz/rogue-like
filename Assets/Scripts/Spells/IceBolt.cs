using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBolt : Spell
{
    private readonly int duration = 6;
    private readonly float freezeChance = 0.3f;

    private CombatSequencer combatSeq;

    public IceBolt()
    {
        this.spellType = SpellType.IceBolt;
        this.targeted = true;
        this.minDamage = 4;
        this.maxDamage = 7;
        this.range = 8;
        this.cooldown = 12;
        this.manaCost = 12;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/IceBolt");
        this.projectileType = ProjectileType.MagicMissile;
        this.castSound = Resources.Load<AudioClip>("Sounds/Fortify");
        combatSeq = GameObject.Find("System Managers").GetComponent<CombatSequencer>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {
        if (!target.GetComponent<CharacterSheet>())
        {
            return false;
        }

        if (!combatSeq.CheckProjectileAttackValidity(caster, target, range))
        {
            return false;
        }

        Attack attack = new(caster, target, minDamage, maxDamage, caster.GetComponent<CharacterSheet>().speed, projectileType);
        attack.AttachStatusEffect(new Frozen(target.GetComponent<CharacterSheet>(), duration), freezeChance);
        combatSeq.AddAttack(attack);

        ResetCooldown(caster);
        return true;
    }
}
