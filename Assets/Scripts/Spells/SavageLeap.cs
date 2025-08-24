using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SavageLeap : Spell
{
    private CombatManager cbm;
    private InventoryManager im;
    private TileManager tileMgr;

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
        tileMgr = managers.GetComponent<TileManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        if (!target.GetComponent<CharacterSheet>())
        {

            return false;
        }

        Equipment mainHandWeapon = (Equipment)im.equipmentSlotsDictionary[ItemSlotType.MainHand].item;

        if (mainHandWeapon == null)
        {

            return false;
        }

        CharacterSheet attackingCharacter = caster.GetComponent<CharacterSheet>();
        CharacterSheet defendingCharacter = target.GetComponent<CharacterSheet>();
        
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
        
        return false;
    }

    private bool TeleportToTarget(CharacterSheet attackingCharacter, CharacterSheet defendingCharacter)
    {

        List<Vector2Int> path = PathFinder.FindPath(attackingCharacter.loc.coord, defendingCharacter.loc.coord, tileMgr.levelCoords);

        // Try to teleport to the last node on the path to the target
        if(attackingCharacter.Teleport(path[^2]))
        {

            return true;
        }

        // If that fails, try to teleport to one of the target's neighbors
        foreach(Vector2Int coord in PathFinder.GetNeighbors(defendingCharacter.loc.coord, tileMgr.levelCoords))
        {

            if(attackingCharacter.Teleport(coord))
            {

                return true;
            }
        }

        return false;
    }

}
