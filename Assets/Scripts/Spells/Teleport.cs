using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Spell
{

    private DungeonManager dum;

    public Teleport()
    {
        
        this.spellName = "Teleport";
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

        if(distance <= range && target.GetComponent<Tile>())
        {

            if(target.GetComponent<Tile>().traversable)
            {

                Vector3 targetPos = new(target.GetComponent<Tile>().coord.x, 0, target.GetComponent<Tile>().coord.y);

                if(!caster.GetComponent<CharacterSheet>().Teleport(targetPos, dum))
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
