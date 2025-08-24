using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : Spell
{

    private int duration = 20;

    public Lift()
    {

        this.spellType = SpellType.Lift;
        this.targeted = true;
        this.cooldown = 15;
        this.manaCost = 3;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Wing");
        this.castSound = Resources.Load<AudioClip>("Sounds/confuse");
    }

    public override bool Cast(GameObject caster, GameObject target = null)
    {

        if (target == null)
        {

            return false;
        }
        
        if (target.TryGetComponent<CharacterSheet>(out var targetCharacterSheet))
        {

            target.GetComponent<StatusEffectManager>().AddEffect(new Levitate(targetCharacterSheet, duration));
            return true;
        }

        return false;
    }
}
