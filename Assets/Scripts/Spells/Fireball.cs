using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
    private CombatManager cbm;

    public Fireball()
    {
        
        this.spellName = "Fireball";
        this.targeted = true;
        this.minDamage = 10;
        this.maxDamage = 20;
        this.range = 6;
        this.cooldown = 10;
        this.manaCost = 10;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Fireball");
        this.projectileType = "Fireball";
        cbm = GameObject.Find("System Managers").GetComponent<CombatManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<Character>())
        {

            float distance = Vector3.Distance(caster.transform.position, target.transform.position);

            if(LineOfSight.HasLOS(caster, target) && distance <= range)
            {

                cbm.combatBuffer.Add(new Attack(caster, target, minDamage, maxDamage, caster.GetComponent<Character>().speed, projectileType));
                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}
