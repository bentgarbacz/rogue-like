using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkAway : Spell
{
    private readonly int duration = 15;
    private DungeonManager dum;

    public SlinkAway()
    {
        
        this.spellType = SpellType.SlinkAway;
        this.targeted = true;
        this.range = 5;
        this.cooldown = 30;
        this.manaCost = 5;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Slink Away");
        this.castSound = Resources.Load<AudioClip>("Sounds/Disappear");

        dum = GameObject.Find("System Managers").GetComponent<DungeonManager>();
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);
        Tile targetTile = target.GetComponent<Tile>();
        CharacterSheet casterCharacterSheet = caster.GetComponent<CharacterSheet>();
        StatusEffectManager statusEffectManager = caster.GetComponent<StatusEffectManager>();

        if(distance <= range && targetTile != null)
        {

            if(targetTile.IsActionable())
            {

                Vector3 targetPos = new Vector3(targetTile.coord.x, 0, targetTile.coord.y);

                if(!casterCharacterSheet.Teleport(targetPos, dum))
                {

                    return false;
                }

                statusEffectManager.AddEffect(new Stealth(casterCharacterSheet, duration, dum));
                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}
