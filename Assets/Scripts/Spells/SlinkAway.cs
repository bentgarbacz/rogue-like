using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkAway : Spell
{
    private readonly int duration = 3;
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

        if(distance <= range && target.GetComponent<Tile>())
        {

            if(target.GetComponent<Tile>().traversable)
            {

                Vector3 targetPos = new Vector3(target.GetComponent<Tile>().coord.x, 0, target.GetComponent<Tile>().coord.y);

                if(!caster.GetComponent<CharacterSheet>().Teleport(targetPos, dum))
                {

                    return false;
                }

                caster.GetComponent<StatusEffectManager>().statusEffects.Add(new Stealth(caster.GetComponent<CharacterSheet>(), duration, dum));
                ResetCooldown(caster);
                return true;
            }
        }

        return false;
    }
}
