using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    private CombatSequencer combatSeq;

    public Fireball()
    {
        
        this.spellType = SpellType.Fireball;
        this.targeted = true;
        this.minDamage = 10;
        this.maxDamage = 20;
        this.range = 6;
        this.cooldown = 10;
        this.manaCost = 10;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Fireball");
        this.projectileType = ProjectileType.Fireball;
        combatSeq = GameObject.Find("System Managers").GetComponent<CombatSequencer>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<CharacterSheet>())
        {

            if(combatSeq.CheckProjectileAttackValidity(caster, target, range))
            {

                Attack attack = new(caster, target, minDamage, maxDamage, caster.GetComponent<CharacterSheet>().speed, projectileType);
                combatSeq.AddAttack(attack);

                ResetCooldown(caster);
                
                return true;
            }
        }

        return false;
    }
}
