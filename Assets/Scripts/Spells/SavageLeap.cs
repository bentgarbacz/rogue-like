using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavageLeap : Spell
{
    private CombatManager cbm;
    private InventoryManager im;
    private DungeonManager dum;

    public SavageLeap()
    {
        
        this.spellName = "Savage Leap";
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

        if(target.GetComponent<Character>())
        {

            Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary["Main Hand"].item;
            Character attackingCharacter = caster.GetComponent<Character>();
            Character defendingCharacter = target.GetComponent<Character>();

            if(mainHandWeapon != null)
            {
                float distance = Vector3.Distance(caster.transform.position, target.transform.position);

                if(mainHandWeapon is not RangedWeapon && LineOfSight.HasLOS(caster, target) && distance <= range)
                {
                    
                    if(!TeleportToTarget(attackingCharacter, defendingCharacter))
                    {

                        return false;
                    }

                    cbm.combatBuffer.Add( new Attack(
                                                caster, 
                                                target,
                                                mainHandWeapon.bonusStatDictionary["Min Damage"], 
                                                mainHandWeapon.bonusStatDictionary["Max Damage"], 
                                                attackingCharacter.speed
                                                ));


                    ResetCooldown(caster);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TeleportToTarget(Character attackingCharacter, Character defendingCharacter)
    {

        List<Vector2Int> path = PathFinder.FindPath(attackingCharacter.coord, defendingCharacter.coord, dum.dungeonCoords);

        // Try to teleport to the last node on the path to the target
        if(attackingCharacter.Teleport(Rules.CoordToPos(path[^2]), dum.occupiedlist))
        {

            return true;
        }

        // If that fails, try to teleport to one of the target's neighbors
        foreach(Vector2Int coord in PathFinder.GetNeighbors(defendingCharacter.coord, dum.dungeonCoords))
        {

            if(attackingCharacter.Teleport(Rules.CoordToPos(coord), dum.occupiedlist))
            {

                return true;
            }
        }

        return false;
    }

}
