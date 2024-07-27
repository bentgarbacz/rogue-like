
using UnityEngine;

public class Heal : Spell
{

    public Heal()
    {

        this.spellName = "Heal";
        this.targeted = false;
        this.cooldown = 30;
        this.manaCost = 20;
        this.sprite = Resources.Load<Sprite>("Pixel Art/Spells/Heal");
        this.castSound = Resources.Load<AudioClip>("Sounds/Heal");
    }

    public override bool Cast(GameObject caster, GameObject target = null)
    {

        caster.GetComponent<Character>().Heal(10);
        ResetCooldown(caster);
        return true;
    }
}
