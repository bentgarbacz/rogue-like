using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    private CombatManager cbm;

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
        cbm = GameObject.Find("System Managers").GetComponent<CombatManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<CharacterSheet>())
        {

            if(cbm.AddProjectileAttack(caster, target, range, minDamage, maxDamage, caster.GetComponent<CharacterSheet>().speed, projectileType))
            {

                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}
