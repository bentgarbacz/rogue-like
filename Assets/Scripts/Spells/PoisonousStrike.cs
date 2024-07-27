using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoisonousStrike : Spell
{  

    private int duration = 5;
    private CombatManager cbm;
    private InventoryManager im;
    private DungeonManager dum;

    public PoisonousStrike()
    {
        
        this.spellName = "Poisonous Strike";
        this.targeted = true;
        this.cooldown = 10;
        this.manaCost = 10;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Poisonous Strike");

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();
        im = managers.GetComponent<InventoryManager>();
        dum = managers.GetComponent<DungeonManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<Character>())
        {

            Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary["Main Hand"].item;
            Character attackingCharacter = caster.GetComponent<Character>();
            Character defendingCharacter = target.GetComponent<Character>();

            if(mainHandWeapon != null)
            {

                int damagePerTurn = (int)Math.Ceiling((double)(mainHandWeapon.bonusStatDictionary["Min Damage"] + mainHandWeapon.bonusStatDictionary["Max Damage"]) / 5);
                
                if(mainHandWeapon is not RangedWeapon && PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords).Contains(attackingCharacter.coord))
                {
                    
                    cbm.combatBuffer.Add( new Attack(
                                                caster, 
                                                target,
                                                mainHandWeapon.bonusStatDictionary["Min Damage"], 
                                                mainHandWeapon.bonusStatDictionary["Max Damage"], 
                                                attackingCharacter.speed
                                                ));

                    //poison target
                    target.GetComponent<StatusEffectManager>().statusEffects.Add(new Poison(defendingCharacter, duration, damagePerTurn, dum));

                    ResetCooldown(caster);
                    return true;

                }else if(mainHandWeapon is RangedWeapon rangedWeapon && mainHandWeapon.bonusStatDictionary["Range"] >= Vector3.Distance(defendingCharacter.transform.position, dum.hero.transform.position))
                {
                    
                    if(LineOfSight.HasLOS(dum.hero, target))
                    {
                        
                        cbm.combatBuffer.Add( new Attack(
                                                    caster, 
                                                    target,
                                                    mainHandWeapon.bonusStatDictionary["Min Damage"], 
                                                    mainHandWeapon.bonusStatDictionary["Max Damage"], 
                                                    attackingCharacter.speed,
                                                    rangedWeapon.projectile
                                                    ));

                        //poison target
                        target.GetComponent<StatusEffectManager>().statusEffects.Add(new Poison(defendingCharacter, duration, damagePerTurn, dum));
                        
                        ResetCooldown(caster);
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
