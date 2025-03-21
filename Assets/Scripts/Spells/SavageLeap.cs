using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SavageLeap : Spell
{
    private CombatManager cbm;
    private InventoryManager im;
    private DungeonManager dum;

    public SavageLeap()
    {
        
        this.spellType = SpellType.SavageLeap;
        this.targeted = true;
        this.cooldown = 10;
        this.manaCost = 10;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Savage Leap");

        GameObject managers = GameObject.Find("System Managers");
        cbm = managers.GetComponent<CombatManager>();
        im = managers.GetComponent<InventoryManager>();
        dum = managers.GetComponent<DungeonManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if(target.GetComponent<CharacterSheet>())
        {

            Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.MainHand].item;
            CharacterSheet attackingCharacter = caster.GetComponent<CharacterSheet>();
            CharacterSheet defendingCharacter = target.GetComponent<CharacterSheet>();
            
            if(mainHandWeapon != null)
            {
                float distance = Vector3.Distance(caster.transform.position, target.transform.position);

                int minDamage = mainHandWeapon.bonusStatDictionary[StatType.MinDamage];
                int maxDamage = mainHandWeapon.bonusStatDictionary[StatType.MaxDamage];

                if(mainHandWeapon is not RangedWeapon && LineOfSight.HasLOS(caster, target) && distance <= range)
                {
                    
                    if(!TeleportToTarget(attackingCharacter, defendingCharacter))
                    {

                        return false;
                    }

                    cbm.AddMeleeAttack(caster, target, minDamage, maxDamage, attackingCharacter.speed );

                    ResetCooldown(caster);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TeleportToTarget(CharacterSheet attackingCharacter, CharacterSheet defendingCharacter)
    {

        List<Vector2Int> path = PathFinder.FindPath(attackingCharacter.coord, defendingCharacter.coord, dum.dungeonCoords);

        // Try to teleport to the last node on the path to the target
        if(attackingCharacter.Teleport(GameFunctions.CoordToPos(path[^2]), dum))
        {

            return true;
        }

        // If that fails, try to teleport to one of the target's neighbors
        foreach(Vector2Int coord in PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords))
        {

            if(attackingCharacter.Teleport(GameFunctions.CoordToPos(coord), dum))
            {

                return true;
            }
        }

        return false;
    }

}
