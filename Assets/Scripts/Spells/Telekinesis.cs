using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Telekinesis : Spell
{

    public Telekinesis()
    {
        
        this.spellType = SpellType.Telekinesis;
        this.targeted = true;
        this.cooldown = 3;
        this.manaCost = 10;
        this.range = 8;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Ball");
    }

    public override bool Cast(GameObject caster, GameObject target)
    {

        Interactable interactable = target.GetComponent<Interactable>();

        if (interactable == null)
        {

            return false;
        }

        if (interactable.Interact())
        {

            ResetCooldown(caster);
            return true;
        }

        return false;
    }
}