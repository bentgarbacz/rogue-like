using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoisonousStrike : Spell
{  

    private int duration = 5;
    private CombatSequencer combatSeq;
    private InventoryManager im;
    private EntityManager entityMgr;

    public PoisonousStrike()
    {
        
        this.spellType = SpellType.PoisonousStrike;
        this.targeted = true;
        this.cooldown = 10;
        this.manaCost = 10;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Poisonous Strike");

        GameObject managers = GameObject.Find("System Managers");
        combatSeq = managers.GetComponent<CombatSequencer>();
        im = managers.GetComponent<InventoryManager>();
        entityMgr = managers.GetComponent<EntityManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<CharacterSheet>())
        {

            Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.MainHand].item;

            CharacterSheet attackingCharacter = caster.GetComponent<CharacterSheet>();
            CharacterSheet defendingCharacter = target.GetComponent<CharacterSheet>();
            
            int minDamage = mainHandWeapon.bonusStatDictionary[StatType.MinDamage];
            int maxDamage = mainHandWeapon.bonusStatDictionary[StatType.MaxDamage];

            if(mainHandWeapon != null)
            {

                int damagePerTurn = (int)Math.Ceiling((double)(minDamage + maxDamage) / 5);
                
                if(mainHandWeapon is not RangedWeapon)
                {
                    
                    if(combatSeq.CheckMeleeAttackValidity(caster, target))
                    {

                        Attack attack = new(caster, target, minDamage, maxDamage, attackingCharacter.speed);
                        attack.AttachStatusEffect(new Poison(defendingCharacter, duration, damagePerTurn), 1f);                        
                        combatSeq.AddAttack(attack);

                        ResetCooldown(caster);
                        return true;
                    }

                }else if(mainHandWeapon is RangedWeapon rangedWeapon)
                {
                    
                    if(combatSeq.CheckProjectileAttackValidity(caster, target, rangedWeapon.bonusStatDictionary[StatType.Range]))
                    {

                        Attack attack = new(caster, target, minDamage, maxDamage, attackingCharacter.speed, rangedWeapon.projectile);
                        attack.AttachStatusEffect(new Poison(defendingCharacter, duration, damagePerTurn), 1f);                        
                        combatSeq.AddAttack(attack);
                        
                        ResetCooldown(caster);
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
