using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Spell
{

    private DungeonManager dum;

    public Teleport()
    {
        
        this.spellType = SpellType.Teleport;
        this.targeted = true;
        this.range = 20;
        this.cooldown = 30;
        this.manaCost = 5;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Teleport");
        this.castSound = Resources.Load<AudioClip>("Sounds/Teleport");

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);
        Tile targetTile = target.GetComponent<Tile>();
        CharacterSheet casterCharacterSheet = caster.GetComponent<CharacterSheet>();

        if(distance <= range && target.GetComponent<Tile>())
        {

            if(targetTile.IsActionable())
            {

                Vector3 targetPos = new(targetTile.coord.x, 0, targetTile.coord.y);

                if(!casterCharacterSheet.Teleport(targetPos, dum))
                {

                    return false;
                }
                
                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}
